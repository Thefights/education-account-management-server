using DTOs.Courses;
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
        ICurrentUserService currentUserService)
        : BaseService<Course, CreateCourseDTO, GetCourseDTO, UpdateCourseDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(Course.School)]),
            ICourseService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<AdminProfile> _adminProfileRepository =
            unitOfWork.Repository<AdminProfile>();
        private readonly IGenericRepository<School> _schoolRepository =
            unitOfWork.Repository<School>();
        private int? _currentSchoolId;

        public override async Task<GetCourseDTO> CreateAsync(
            CreateCourseDTO createDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            var schoolId = await ResolveSchoolIdAsync(createDTO.SchoolId, cancellationToken);

            var courseId = await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var course = _mapper.MapFromCreateDTO(createDTO);
                    course.SchoolId = schoolId;
                    course.CourseCode = await GenerateUniqueCourseCodeAsync(schoolId, token);
                    course.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(
                        _repository,
                        course,
                        cancellationToken: token);

                    await _repository.AddAsync(course, token);
                    await _unitOfWork.SaveChangeAsync(token);
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
            var scopedSchoolId = await GetScopedSchoolIdAsync(cancellationToken);
            var targetSchoolId = await ResolveSchoolIdAsync(updateDTO.SchoolId, cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var course = await _repository.Query(tracking: true)
                        .FirstOrDefaultAsync(
                            entity => entity.Id == id &&
                                (!scopedSchoolId.HasValue || entity.SchoolId == scopedSchoolId.Value),
                            token)
                        ?? throw new DataNotFoundException(typeof(Course), id);

                    _mapper.MapFromUpdateDTO(updateDTO, course);
                    course.SchoolId = targetSchoolId;
                    course.TryValidate();
                    await ValidateCourseCodeAsync(course, course.Id, token);
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

        public override async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await GetScopedSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var course = await _repository.Query(tracking: true)
                        .FirstOrDefaultAsync(
                            entity => entity.Id == id &&
                                (!schoolId.HasValue || entity.SchoolId == schoolId.Value),
                            token)
                        ?? throw new DataNotFoundException(typeof(Course), id);

                    _repository.Remove(course);
                    await _unitOfWork.SaveChangeAsync(token);
                },
                cancellationToken);
        }

        public override async Task DeleteSelectedIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(ids);
            var schoolId = await GetScopedSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var courses = await _repository.Query(tracking: true)
                        .Where(course => ids.Contains(course.Id) &&
                            (!schoolId.HasValue || course.SchoolId == schoolId.Value))
                        .ToListAsync(token);

                    if (courses.Count != ids.Count)
                    {
                        var foundIds = courses.Select(course => course.Id).ToHashSet();
                        var firstNotFoundId = ids.First(id => !foundIds.Contains(id));
                        throw new DataNotFoundException(typeof(Course), firstNotFoundId);
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
            var schoolId = await GetScopedSchoolIdAsync(cancellationToken);
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);
            var (total, courses) = await _repository.GetProjectedPaginatedAsync(
                _mapper.ProjectToGetDTO,
                course => !schoolId.HasValue || course.SchoolId == schoolId.Value,
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
            var schoolId = await GetScopedSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                course => !schoolId.HasValue || course.SchoolId == schoolId.Value,
                _includes,
                cancellationToken);
        }

        public override async Task<List<GetCourseDTO>> GetAllByIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(ids);
            var schoolId = await GetScopedSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                course => ids.Contains(course.Id) &&
                    (!schoolId.HasValue || course.SchoolId == schoolId.Value),
                _includes,
                cancellationToken);
        }

        public override async Task<GetCourseDTO> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await GetScopedSchoolIdAsync(cancellationToken);
            return await _repository.FirstOrDefaultProjectedAsync(
                    _mapper.ProjectToGetDTO,
                    course => course.Id == id &&
                        (!schoolId.HasValue || course.SchoolId == schoolId.Value),
                    _includes,
                    cancellationToken)
                ?? throw new DataNotFoundException(typeof(Course), id);
        }

        private async Task ValidateCourseCodeAsync(
            Course course,
            int? excludedId = null,
            CancellationToken cancellationToken = default)
        {
            var exists = await _repository.AnyAsync(
                item => item.SchoolId == course.SchoolId
                    && item.CourseCode == course.CourseCode
                    && (!excludedId.HasValue || item.Id != excludedId.Value),
                cancellationToken);

            if (exists)
            {
                throw new DataConflictException(
                    $"{nameof(Course.CourseCode)} already exists in the selected school.");
            }
        }

        private async Task<string> GenerateUniqueCourseCodeAsync(
            int schoolId,
            CancellationToken cancellationToken)
        {
            const int maxAttempts = 10;
            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                var courseCode = CourseCodeGenerator.Generate();
                if (!await _repository.AnyAsync(
                        course => course.SchoolId == schoolId && course.CourseCode == courseCode,
                        cancellationToken))
                {
                    return courseCode;
                }
            }

            throw new DataConflictException("Unable to generate a unique course code.");
        }

        private async Task<int?> GetScopedSchoolIdAsync(CancellationToken cancellationToken)
        {
            if (_currentUserService.Role == UserRole.SystemAdmin)
            {
                return null;
            }

            if (_currentUserService.Role != UserRole.SchoolAdmin)
            {
                throw new UnauthorizedAccessException("Only system or school administrators can manage courses.");
            }

            if (_currentSchoolId.HasValue)
            {
                return _currentSchoolId.Value;
            }

            var schoolId = await _adminProfileRepository.Query()
                .Where(profile => profile.UserId == _currentUserService.UserId)
                .Select(profile => profile.SchoolId)
                .SingleOrDefaultAsync(cancellationToken);

            _currentSchoolId = schoolId
                ?? throw new UnauthorizedAccessException(
                    "The school administrator is not assigned to a school.");

            return _currentSchoolId.Value;
        }

        private async Task<int> ResolveSchoolIdAsync(
            int requestedSchoolId,
            CancellationToken cancellationToken)
        {
            var scopedSchoolId = await GetScopedSchoolIdAsync(cancellationToken);
            if (scopedSchoolId.HasValue)
            {
                return scopedSchoolId.Value;
            }

            if (requestedSchoolId <= 0)
            {
                throw new ValidationFailureException(
                    nameof(requestedSchoolId),
                    "SchoolId is required for SystemAdmin.");
            }

            if (!await _schoolRepository.AnyAsync(
                    school => school.Id == requestedSchoolId,
                    cancellationToken))
            {
                throw new DataNotFoundException(typeof(School), requestedSchoolId);
            }

            return requestedSchoolId;
        }
    }
}
