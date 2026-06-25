using DTOs.Courses;
using Interfaces.Audit;
using Interfaces.Courses;
using Results;
using Services.Base;
using Services.Courses.Utils;
using Validators;

namespace Services.Courses
{
    public class CourseService(
        IUnitOfWork unitOfWork,
        CourseMapper mapper,
        SchoolScopeResolver schoolScopeResolver,
        IAuditLogWriter auditLogWriter,
        TimeProvider timeProvider)
        : BaseService<Course, CreateCourseDTO, GetCourseDTO, UpdateCourseDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(Course.School)]),
            ICourseService
    {
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly TimeProvider _timeProvider = timeProvider;
        private readonly IGenericRepository<AiAssistantSetting> _settingRepository =
            unitOfWork.Repository<AiAssistantSetting>();
        private readonly IGenericRepository<SchoolStudent> _schoolStudentRepository =
            unitOfWork.Repository<SchoolStudent>();
        private readonly IGenericRepository<School> _schoolRepository =
            unitOfWork.Repository<School>();
        private readonly IGenericRepository<Enrollment> _enrollmentRepository =
            unitOfWork.Repository<Enrollment>();

        public override async Task<GetCourseDTO> CreateAsync(
            CreateCourseDTO createDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var courseId = await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    ValidateCreateStudentIds(createDTO.SchoolStudentIds);
                    var course = _mapper.MapFromCreateDTO(createDTO);
                    course.SchoolId = schoolId;
                    course.Status = CourseStatus.Draft;
                    course.FasApplicationDueDate = course.EnrollmentDeadline;
                    var taxRate = await GetTaxRateAsync(token);
                    course.GstAmount = CourseFeeCalculator.CalculateTaxAmount(
                        course.CourseFeeAmount,
                        course.MiscFeeAmount,
                        taxRate);
                    CourseDateTimeHelper.NormalizeToUtc(course);
                    course.CourseCode = await CourseCodeGenerator.GenerateUniqueAsync(
                        _repository,
                        schoolId,
                        _timeProvider.GetUtcNow().UtcDateTime,
                        cancellationToken: token);
                    course.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(
                        _repository,
                        course,
                        cancellationToken: token);

                    await _repository.AddAsync(course, token);
                    await _unitOfWork.SaveChangeAsync(token);
                    if (createDTO.SchoolStudentIds.Count > 0)
                    {
                        await CreateInitialEnrollmentsAsync(course, createDTO.SchoolStudentIds, schoolId, token);
                    }
                    return course.Id;
                },
                cancellationToken);

            return await GetByIdAsync(courseId, cancellationToken);
        }

        public override async Task<GetCourseDTO> UpdateAsync(
            int id,
            UpdateCourseDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var course = await GetTrackedScopedCourseAsync(id, schoolId, token);
                    CourseConcurrencyHelper.Validate(updateDTO.RowVersion, course.RowVersion);

                    if (course.Status is CourseStatus.Upcoming or CourseStatus.InProgress or CourseStatus.Closed)
                    {
                        throw new DataConflictException(
                            $"A course in {course.Status} status cannot be updated.");
                    }

                    if (course.Status == CourseStatus.Enrolling)
                    {
                        ValidateEnrollingLockedFields(course, updateDTO);
                    }

                    _mapper.MapFromUpdateDTO(updateDTO, course);
                    course.FasApplicationDueDate = course.EnrollmentDeadline;
                    var taxRate = await GetTaxRateAsync(token);
                    course.GstAmount = CourseFeeCalculator.CalculateTaxAmount(
                        course.CourseFeeAmount,
                        course.MiscFeeAmount,
                        taxRate);
                    CourseDateTimeHelper.NormalizeToUtc(course);
                    course.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(
                        _repository,
                        course,
                        course.Id,
                        token);

                    _repository.Update(course);
                    await _unitOfWork.SaveChangeAsync(token);
                },
                cancellationToken);

            return await GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<GetCourseDTO>> PublishAsync(
            PublishCourseDTO publishDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(publishDTO);
            var ids = publishDTO.Ids;
            if (ids.Count == 0 || ids.Any(id => id <= 0))
            {
                throw new ValidationFailureException(
                    nameof(ids),
                    "At least one valid course ID is required.");
            }

            if (ids.Distinct().Count() != ids.Count)
            {
                throw new ValidationFailureException(
                    nameof(ids),
                    "Duplicate course IDs are not allowed.");
            }

            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var utcNow = _timeProvider.GetUtcNow().UtcDateTime;

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var courses = await _repository.Query(tracking: true)
                        .Where(course => ids.Contains(course.Id) && course.SchoolId == schoolId)
                        .ToListAsync(token);

                    if (courses.Count != ids.Count)
                    {
                        var foundIds = courses.Select(course => course.Id).ToHashSet();
                        var firstNotFoundId = ids.First(id => !foundIds.Contains(id));
                        throw new DataNotFoundException(typeof(Course), firstNotFoundId);
                    }

                    foreach (var course in courses)
                    {
                        if (course.Status != CourseStatus.Draft)
                        {
                            throw new DataConflictException(
                                $"Course {course.Id} is not in Draft status.");
                        }

                        if (course.EnrollmentDeadline <= utcNow)
                        {
                            throw new ValidationFailureException(
                                nameof(Course.EnrollmentDeadline),
                                $"Course {course.Id} enrollment deadline must be in the future when publishing.");
                        }

                        course.TryValidate();
                        course.Status = CourseStatus.Enrolling;
                        await _auditLogWriter.LogAsync(
                            AuditLogCategory.StatusChange,
                            $"Course {course.CourseCode} published: Draft to Enrolling.",
                            cancellationToken: token);
                    }

                    _repository.UpdateRange(courses);
                    await _unitOfWork.SaveChangeAsync(token);
                },
                cancellationToken);

            return await GetAllByIdsAsync(ids, cancellationToken);
        }

        public async Task<GetCourseDTO> DuplicateAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var duplicateId = await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var source = await _repository.Query()
                        .FirstOrDefaultAsync(
                            course => course.Id == id && course.SchoolId == schoolId,
                            token)
                        ?? throw new DataNotFoundException(typeof(Course), id);

                    var duplicate = new Course
                    {
                        SchoolId = schoolId,
                        Status = CourseStatus.Draft,
                        CourseCode = await CourseCodeGenerator.GenerateUniqueAsync(
                            _repository,
                            schoolId,
                            _timeProvider.GetUtcNow().UtcDateTime,
                            cancellationToken: token),
                        CourseName = await GenerateDuplicateCourseNameAsync(
                            source.CourseName,
                            token),
                        CourseFeeAmount = source.CourseFeeAmount,
                        MiscFeeAmount = source.MiscFeeAmount,
                        GstAmount = source.GstAmount,
                        EnrollmentDeadline = source.EnrollmentDeadline,
                        FasApplicationDueDate = source.EnrollmentDeadline,
                        StartDate = source.StartDate,
                        EndDate = source.EndDate
                    };

                    duplicate.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(
                        _repository,
                        duplicate,
                        cancellationToken: token);
                    await _repository.AddAsync(duplicate, token);
                    await _unitOfWork.SaveChangeAsync(token);

                    return duplicate.Id;
                },
                cancellationToken);

            return await GetByIdAsync(duplicateId, cancellationToken);
        }

        public async Task DeleteAsync(
            int id,
            byte[] rowVersion,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var course = await GetTrackedScopedCourseWithEnrollmentsAsync(id, schoolId, token);
                    CourseConcurrencyHelper.Validate(rowVersion, course.RowVersion);
                    ValidateCanDelete(course);

                    _repository.Remove(course);
                    await _unitOfWork.SaveChangeAsync(token);
                },
                cancellationToken);
        }

        public async Task DeleteSelectedAsync(
            DeleteSelectedCoursesDTO deleteDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(deleteDTO);
            if (deleteDTO.Items.Count == 0)
            {
                throw new ValidationFailureException(
                    nameof(deleteDTO.Items),
                    "At least one course is required.");
            }

            if (deleteDTO.Items.Any(item => item.Id <= 0))
            {
                throw new ValidationFailureException(
                    nameof(DeleteCourseItemDTO.Id),
                    "Course IDs must be greater than zero.");
            }

            var ids = deleteDTO.Items.Select(item => item.Id).ToList();
            if (ids.Distinct().Count() != ids.Count)
            {
                throw new ValidationFailureException(
                    nameof(DeleteCourseItemDTO.Id),
                    "Duplicate course IDs are not allowed.");
            }

            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var courses = await _repository.Query(tracking: true)
                        .Where(course => ids.Contains(course.Id) && course.SchoolId == schoolId)
                        .Include(course => course.Enrollments)
                        .ThenInclude(enrollment => enrollment.Charge)
                        .ToListAsync(token);

                    if (courses.Count != ids.Count)
                    {
                        var foundIds = courses.Select(course => course.Id).ToHashSet();
                        var firstNotFoundId = ids.First(courseId => !foundIds.Contains(courseId));
                        throw new DataNotFoundException(typeof(Course), firstNotFoundId);
                    }

                    var rowVersions = deleteDTO.Items.ToDictionary(item => item.Id, item => item.RowVersion);
                    foreach (var course in courses)
                    {
                        CourseConcurrencyHelper.Validate(rowVersions[course.Id], course.RowVersion);
                        ValidateCanDelete(course);
                    }

                    _repository.RemoveRange(courses);
                    await _unitOfWork.SaveChangeAsync(token);
                },
                cancellationToken);
        }

        public override async Task<PaginationResult<GetCourseDTO>> GetAllPaginatedAsync(
            FilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);
            var (total, courses) = await _repository.GetProjectedPaginatedAsync(
                ProjectToGetDTO,
                course => course.SchoolId == schoolId,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression,
                filterDTO.Page,
                pageSize,
                _includes,
                cancellationToken);

            return new PaginationResult<GetCourseDTO>(total, pageSize, courses);
        }

        public override async Task<List<GetCourseDTO>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                ProjectToGetDTO,
                course => course.SchoolId == schoolId,
                _includes,
                cancellationToken);
        }

        public override async Task<List<GetCourseDTO>> GetAllByIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(ids);
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                ProjectToGetDTO,
                course => ids.Contains(course.Id) && course.SchoolId == schoolId,
                _includes,
                cancellationToken);
        }

        public override async Task<GetCourseDTO> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.FirstOrDefaultProjectedAsync(
                    ProjectToGetDTO,
                    course => course.Id == id && course.SchoolId == schoolId,
                    _includes,
                    cancellationToken)
                ?? throw new DataNotFoundException(typeof(Course), id);
        }

        private async Task<Course> GetTrackedScopedCourseAsync(
            int id,
            int schoolId,
            CancellationToken cancellationToken)
        {
            return await _repository.Query(tracking: true)
                .FirstOrDefaultAsync(
                    course => course.Id == id && course.SchoolId == schoolId,
                    cancellationToken)
                ?? throw new DataNotFoundException(typeof(Course), id);
        }

        private async Task<Course> GetTrackedScopedCourseWithEnrollmentsAsync(
            int id,
            int schoolId,
            CancellationToken cancellationToken)
        {
            return await _repository.Query(tracking: true)
                .Include(course => course.Enrollments)
                .ThenInclude(enrollment => enrollment.Charge)
                .FirstOrDefaultAsync(
                    course => course.Id == id && course.SchoolId == schoolId,
                    cancellationToken)
                ?? throw new DataNotFoundException(typeof(Course), id);
        }

        private static void ValidateCanDelete(Course course)
        {
            if (course.Status != CourseStatus.Draft)
            {
                throw new DataConflictException("Only a draft course can be deleted.");
            }

            if (course.Enrollments.Any(enrollment => enrollment.Charge != null))
            {
                throw new DataConflictException(
                    "A draft course with a generated charge cannot be deleted.");
            }
        }

        private static void ValidateEnrollingLockedFields(
            Course course,
            UpdateCourseDTO updateDTO)
        {
            var enrollmentDeadline = CourseDateTimeHelper.NormalizeToUtc(
                updateDTO.EnrollmentDeadline,
                nameof(updateDTO.EnrollmentDeadline));
            var startDate = CourseDateTimeHelper.NormalizeToUtc(
                updateDTO.StartDate,
                nameof(updateDTO.StartDate));
            var endDate = CourseDateTimeHelper.NormalizeToUtc(
                updateDTO.EndDate,
                nameof(updateDTO.EndDate));

            if (course.CourseFeeAmount != updateDTO.CourseFeeAmount
                || course.MiscFeeAmount != updateDTO.MiscFeeAmount
                || course.EnrollmentDeadline != enrollmentDeadline
                || course.StartDate != startDate
                || course.EndDate != endDate)
            {
                throw new DataConflictException(
                    "Fees and deadlines cannot be changed after a course is published.");
            }
        }

        private async Task<decimal> GetTaxRateAsync(CancellationToken cancellationToken)
        {
            var taxRate = await _settingRepository.Query()
                .OrderBy(setting => setting.Id)
                .Select(setting => setting.TaxRate)
                .FirstOrDefaultAsync(cancellationToken);
            return taxRate;
        }

        private static IQueryable<GetCourseDTO> ProjectToGetDTO(IQueryable<Course> query)
        {
            return query.Select(course => new GetCourseDTO
            {
                Id = course.Id,
                SchoolId = course.SchoolId,
                SchoolName = course.School.SchoolName,
                Status = course.Status.ToString(),
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                CourseFeeAmount = course.CourseFeeAmount,
                MiscFeeAmount = course.MiscFeeAmount,
                GstAmount = course.GstAmount,
                EnrollmentDeadline = course.EnrollmentDeadline,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                EnrollmentCount = course.Enrollments.Count,
                RowVersion = course.RowVersion,
                ApplicableFasSchemes = course.FasSchemeCourses
                    .OrderBy(item => item.FasScheme.SchemeName)
                    .Select(item => new GetCourseFasSchemeDTO
                    {
                        Id = item.FasScheme.Id,
                        SchemeCode = item.FasScheme.SchemeCode,
                        SchemeName = item.FasScheme.SchemeName,
                        Status = item.FasScheme.Status.ToString(),
                        SubsidyType = item.FasScheme.SubsidyType.ToString(),
                        IsPerComponent = item.FasScheme.IsPerComponent
                    })
                    .ToList()
            });
        }

        private static void ValidateCreateStudentIds(List<int> schoolStudentIds)
        {
            if (schoolStudentIds.Any(id => id <= 0))
            {
                throw new ValidationFailureException(
                    nameof(CreateCourseDTO.SchoolStudentIds),
                    "School student IDs must be greater than zero.");
            }

            if (schoolStudentIds.Distinct().Count() != schoolStudentIds.Count)
            {
                throw new ValidationFailureException(
                    nameof(CreateCourseDTO.SchoolStudentIds),
                    "Duplicate school student IDs are not allowed.");
            }
        }

        private async Task CreateInitialEnrollmentsAsync(
            Course course,
            List<int> schoolStudentIds,
            int schoolId,
            CancellationToken cancellationToken)
        {
            var students = await _schoolStudentRepository.Query()
                .Where(student => schoolStudentIds.Contains(student.Id)
                    && student.SchoolId == schoolId)
                .Include(student => student.EducationAccount)
                    .ThenInclude(account => account.Citizen)
                .ToListAsync(cancellationToken);

            if (students.Count != schoolStudentIds.Count)
            {
                var foundIds = students.Select(student => student.Id).ToHashSet();
                var firstNotFoundId = schoolStudentIds.First(id => !foundIds.Contains(id));
                throw new DataNotFoundException(typeof(SchoolStudent), firstNotFoundId);
            }

            var inactiveStudent = students.FirstOrDefault(student =>
                student.Status != SchoolStudentStatus.Active
                || student.EducationAccount.Status == EducationAccountStatus.Closed);
            if (inactiveStudent != null)
            {
                throw new DataConflictException(
                    $"School student {inactiveStudent.Id} is not eligible for enrollment.");
            }

            var schoolName = await _schoolRepository.Query()
                .Where(school => school.Id == schoolId)
                .Select(school => school.SchoolName)
                .FirstAsync(cancellationToken);

            var enrollments = students.Select(student =>
            {
                var citizen = student.EducationAccount.Citizen;
                var enrollment = new Enrollment
                {
                    CourseId = course.Id,
                    SchoolStudentId = student.Id,
                    SchoolNameSnapshot = schoolName,
                    CourseNameSnapshot = course.CourseName,
                    CitizenNricSnapshot = citizen.Nric,
                    CitizenFullNameSnapshot = citizen.FullName,
                    CitizenEmailSnapshot = citizen.Email,
                    CitizenPhoneNumberSnapshot = citizen.PhoneNumber,
                    AccountNumberSnapshot = student.EducationAccount.AccountNumber
                };
                enrollment.TryValidate();
                return enrollment;
            }).ToList();

            await _enrollmentRepository.AddRangeAsync(enrollments, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        private async Task<string> GenerateDuplicateCourseNameAsync(
            string sourceName,
            CancellationToken cancellationToken)
        {
            const int maxLength = 150;
            var baseName = $"Copy of {sourceName}";
            if (baseName.Length > maxLength)
            {
                baseName = baseName[..maxLength];
            }

            if (!await CourseNameExistsAsync(baseName, cancellationToken))
            {
                return baseName;
            }

            for (var suffix = 2; ; suffix++)
            {
                var suffixText = $" ({suffix})";
                var trimmedBaseName = baseName.Length + suffixText.Length > maxLength
                    ? baseName[..(maxLength - suffixText.Length)]
                    : baseName;
                var candidate = $"{trimmedBaseName}{suffixText}";
                if (!await CourseNameExistsAsync(candidate, cancellationToken))
                {
                    return candidate;
                }
            }
        }

        private async Task<bool> CourseNameExistsAsync(
            string courseName,
            CancellationToken cancellationToken)
        {
            return await _repository.Query()
                .AnyAsync(course => course.CourseName == courseName, cancellationToken);
        }

    }
}
