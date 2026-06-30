using DTOs.Enrollments;
using DTOs.SchoolStudents;
using Filters.SchoolStudents;
using Interfaces.Audit;
using Interfaces.Enrollments;
using Mappers.Enrollments;
using Mappers.SchoolStudents;
using Results;
using Services.Base;

namespace Services.Enrollments
{
    public class EnrollmentService(
        IUnitOfWork unitOfWork,
        EnrollmentMapper mapper,
        SchoolStudentMapper schoolStudentMapper,
        SchoolScopeResolver schoolScopeResolver,
        TimeProvider timeProvider,
        IManagementActionLogService managementActionLogService)
        : BaseGetService<Enrollment, GetEnrollmentDTO>(unitOfWork, mapper),
            IEnrollmentService
    {
        private readonly EnrollmentMapper _mapper = mapper;
        private readonly SchoolStudentMapper _schoolStudentMapper = schoolStudentMapper;
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly TimeProvider _timeProvider = timeProvider;
        private readonly IManagementActionLogService _managementActionLogService = managementActionLogService;
        private readonly IGenericRepository<Course> _courseRepository = unitOfWork.Repository<Course>();
        private readonly IGenericRepository<SchoolStudent> _schoolStudentRepository = unitOfWork.Repository<SchoolStudent>();

        public async Task<List<GetEnrollmentDTO>> AssignAsync(
            AssignEnrollmentsDTO assignDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(assignDTO);
            if (assignDTO.CourseId <= 0)
            {
                throw new ValidationFailureException(
                    nameof(assignDTO.CourseId),
                    "A valid course ID is required.");
            }

            ValidateIds(assignDTO.SchoolStudentIds, nameof(assignDTO.SchoolStudentIds));

            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var enrollmentIds = await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var course = await _courseRepository.Query(tracking: true)
                        .Include(item => item.School)
                        .FirstOrDefaultAsync(
                            item => item.Id == assignDTO.CourseId && item.SchoolId == schoolId,
                            token)
                        ?? throw new DataNotFoundException(typeof(Course), assignDTO.CourseId);

                    ValidateCourseCanManageEnrollments(
                        course,
                        _timeProvider.GetUtcNow().UtcDateTime);

                    var students = await _schoolStudentRepository.Query()
                        .Where(student => assignDTO.SchoolStudentIds.Contains(student.Id)
                            && student.SchoolId == schoolId)
                        .Include(student => student.EducationAccount)
                            .ThenInclude(account => account.Citizen)
                        .ToListAsync(token);

                    if (students.Count != assignDTO.SchoolStudentIds.Count)
                    {
                        var foundIds = students.Select(student => student.Id).ToHashSet();
                        var firstNotFoundId = assignDTO.SchoolStudentIds.First(id => !foundIds.Contains(id));
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

                    var existingEnrollment = await _repository.Query()
                        .FirstOrDefaultAsync(
                            enrollment => enrollment.CourseId == course.Id
                                && assignDTO.SchoolStudentIds.Contains(enrollment.SchoolStudentId),
                            token);
                    if (existingEnrollment != null)
                    {
                        throw new DataConflictException(
                            $"School student {existingEnrollment.SchoolStudentId} is already assigned to this course.");
                    }

                    var enrollments = students.Select(student =>
                    {
                        var citizen = student.EducationAccount.Citizen;
                        var enrollment = new Enrollment
                        {
                            CourseId = course.Id,
                            SchoolStudentId = student.Id,
                            SchoolNameSnapshot = course.School.SchoolName,
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

                    await _repository.AddRangeAsync(enrollments, token);
                    course.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
                    await _unitOfWork.SaveChangeAsync(token);
                    return enrollments.Select(enrollment => enrollment.Id).ToList();
                },
                cancellationToken);

            return await GetAllByIdsAsync(enrollmentIds, cancellationToken);
        }

        public async Task<PaginationResult<GetSchoolStudentDTO>> GetEligibleStudentsAsync(
            int courseId,
            SchoolStudentFilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);
            if (courseId <= 0)
            {
                throw new ValidationFailureException(nameof(courseId), "A valid course ID is required.");
            }

            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var course = await _courseRepository.Query()
                .FirstOrDefaultAsync(
                    item => item.Id == courseId && item.SchoolId == schoolId,
                    cancellationToken)
                ?? throw new DataNotFoundException(typeof(Course), courseId);

            ValidateCourseCanManageEnrollments(
                course,
                _timeProvider.GetUtcNow().UtcDateTime);

            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);
            var (total, students) = await _schoolStudentRepository.GetProjectedPaginatedAsync(
                _schoolStudentMapper.ProjectToGetDTO,
                student => student.SchoolId == schoolId
                    && student.Status == SchoolStudentStatus.Active
                    && student.EducationAccount.Status != EducationAccountStatus.Closed
                    && !student.Enrollments.Any(enrollment => enrollment.CourseId == courseId),
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression,
                filterDTO.Page,
                pageSize,
                cancellationToken: cancellationToken);

            return new PaginationResult<GetSchoolStudentDTO>(total, pageSize, students);
        }

        public async Task RemoveSelectedAsync(
            RemoveSelectedEnrollmentsDTO removeDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(removeDTO);
            var batchId = Guid.NewGuid();
            ValidateIds(removeDTO.Ids, nameof(removeDTO.Ids));

            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var enrollments = await _repository.Query(tracking: true)
                        .Where(item => removeDTO.Ids.Contains(item.Id)
                            && item.Course.SchoolId == schoolId)
                        .Include(item => item.Course)
                        .Include(item => item.Charge)
                        .ToListAsync(token);

                    if (enrollments.Count != removeDTO.Ids.Count)
                    {
                        var foundIds = enrollments.Select(enrollment => enrollment.Id).ToHashSet();
                        var firstNotFoundId = removeDTO.Ids.First(id => !foundIds.Contains(id));
                        throw new DataNotFoundException(typeof(Enrollment), firstNotFoundId);
                    }

                    foreach (var enrollment in enrollments)
                    {
                        ValidateCanRemove(
                            enrollment,
                            _timeProvider.GetUtcNow().UtcDateTime);
                        await _managementActionLogService.LogAsync(
                            batchId,
                            ManagementActionEntityType.Enrollment,
                            enrollment.Id,
                            ManagementAction.Delete,
                            removeDTO.Reason,
                            enrollment.Status.ToString(),
                            null,
                            cancellationToken: token);
                    }

                    _repository.RemoveRange(enrollments);
                    foreach (var course in enrollments.Select(item => item.Course).DistinctBy(course => course.Id))
                    {
                        course.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
                    }

                    await _unitOfWork.SaveChangeAsync(token);
                },
                cancellationToken);
        }

        public async Task WithdrawAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var enrollment = await _repository.Query(tracking: true)
                .Include(item => item.Course)
                .FirstOrDefaultAsync(
                    item => item.Id == id && item.Course.SchoolId == schoolId,
                    cancellationToken)
                ?? throw new DataNotFoundException(typeof(Enrollment), id);

            if (enrollment.Course.Status is not CourseStatus.Upcoming and not CourseStatus.InProgress)
            {
                throw new DataConflictException(
                    "A student can only withdraw from an Upcoming or In Progress course.");
            }

            if (enrollment.Status == EnrollmentStatus.Withdrawn)
            {
                throw new DataConflictException("The student has already withdrawn from this course.");
            }

            enrollment.Status = EnrollmentStatus.Withdrawn;
            enrollment.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            _repository.Update(enrollment);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public override async Task<PaginationResult<GetEnrollmentDTO>> GetAllPaginatedAsync(
            FilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);
            var (total, enrollments) = await _repository.GetProjectedPaginatedAsync(
                _mapper.ProjectToGetDTO,
                enrollment => enrollment.Course.SchoolId == schoolId,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression,
                filterDTO.Page,
                pageSize,
                cancellationToken: cancellationToken);

            return new PaginationResult<GetEnrollmentDTO>(total, pageSize, enrollments);
        }

        public override async Task<List<GetEnrollmentDTO>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                enrollment => enrollment.Course.SchoolId == schoolId,
                cancellationToken: cancellationToken);
        }

        public override async Task<List<GetEnrollmentDTO>> GetAllByIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(ids);
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                enrollment => ids.Contains(enrollment.Id)
                    && enrollment.Course.SchoolId == schoolId,
                cancellationToken: cancellationToken);
        }

        public override async Task<GetEnrollmentDTO> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.FirstOrDefaultProjectedAsync(
                    _mapper.ProjectToGetDTO,
                    enrollment => enrollment.Id == id
                        && enrollment.Course.SchoolId == schoolId,
                    cancellationToken: cancellationToken)
                ?? throw new DataNotFoundException(typeof(Enrollment), id);
        }

        private static void ValidateCanRemove(Enrollment enrollment, DateTime utcNow)
        {
            ValidateCourseCanManageEnrollments(enrollment.Course, utcNow);
            if (enrollment.Charge != null)
            {
                throw new DataConflictException(
                    "An enrollment with a generated charge cannot be removed.");
            }
        }

        private static void ValidateCourseCanManageEnrollments(Course course, DateTime utcNow)
        {
            if (course.Status is not CourseStatus.Draft and not CourseStatus.Enrolling)
            {
                throw new DataConflictException(
                    "The enrollment list can only be changed while the course is in Draft or Enrolling status.");
            }

            if (course.Status == CourseStatus.Enrolling && course.EnrollmentDeadline <= utcNow)
            {
                throw new DataConflictException(
                    "The enrollment list cannot be changed after the enrollment deadline.");
            }
        }

        private static void ValidateIds(List<int> ids, string fieldName)
        {
            if (ids.Count == 0 || ids.Any(id => id <= 0))
            {
                throw new ValidationFailureException(
                    fieldName,
                    "At least one valid ID is required.");
            }

            if (ids.Distinct().Count() != ids.Count)
            {
                throw new ValidationFailureException(fieldName, "Duplicate IDs are not allowed.");
            }
        }
    }
}
