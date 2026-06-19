using DTOs.CourseManagement;
using Interfaces.CourseManagement;
using Results;
using Services.Base;
using Validators;

namespace Services.CourseManagement
{
    public class CourseManagementService(
        IUnitOfWork unitOfWork,
        CourseManagementMapper mapper,
        ICurrentUserService currentUserService)
        : BaseService<Course, CreateCourseManagementDTO, GetCourseManagementDTO, UpdateCourseManagementDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(Course.School)]),
            ICourseManagementService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<AdminProfile> _adminProfileRepository =
            unitOfWork.Repository<AdminProfile>();
        private int? _currentSchoolId;

        public override async Task<GetCourseManagementDTO> CreateAsync(
            CreateCourseManagementDTO createDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            var schoolId = await GetCurrentSchoolIdAsync(cancellationToken);

            var courseId = await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var course = _mapper.MapFromCreateDTO(createDTO);
                    course.SchoolId = schoolId;
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

        public override async Task<GetCourseManagementDTO> UpdateAsync(
            int id,
            UpdateCourseManagementDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            var schoolId = await GetCurrentSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var course = await _repository.Query(tracking: true)
                        .FirstOrDefaultAsync(
                            entity => entity.Id == id && entity.SchoolId == schoolId,
                            token)
                        ?? throw new DataNotFoundException(typeof(Course), id);

                    _mapper.MapFromUpdateDTO(updateDTO, course);
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

        public override async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await GetCurrentSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var course = await _repository.Query(tracking: true)
                        .FirstOrDefaultAsync(
                            entity => entity.Id == id && entity.SchoolId == schoolId,
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
            var schoolId = await GetCurrentSchoolIdAsync(cancellationToken);

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

                    _repository.RemoveRange(courses);
                    await _unitOfWork.SaveChangeAsync(token);
                },
                cancellationToken);
        }

        public override async Task<PaginationResult<GetCourseManagementDTO>> GetAllPaginatedAsync(
            FilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);
            var schoolId = await GetCurrentSchoolIdAsync(cancellationToken);
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);
            var (total, courses) = await _repository.GetProjectedPaginatedAsync(
                _mapper.ProjectToGetDTO,
                course => course.SchoolId == schoolId,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression,
                filterDTO.Page,
                pageSize,
                _includes,
                cancellationToken);

            return new PaginationResult<GetCourseManagementDTO>(total, pageSize, courses);
        }

        public override async Task<List<GetCourseManagementDTO>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var schoolId = await GetCurrentSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                course => course.SchoolId == schoolId,
                _includes,
                cancellationToken);
        }

        public override async Task<List<GetCourseManagementDTO>> GetAllByIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(ids);
            var schoolId = await GetCurrentSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                course => ids.Contains(course.Id) && course.SchoolId == schoolId,
                _includes,
                cancellationToken);
        }

        public override async Task<GetCourseManagementDTO> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await GetCurrentSchoolIdAsync(cancellationToken);
            return await _repository.FirstOrDefaultProjectedAsync(
                    _mapper.ProjectToGetDTO,
                    course => course.Id == id && course.SchoolId == schoolId,
                    _includes,
                    cancellationToken)
                ?? throw new DataNotFoundException(typeof(Course), id);
        }

        private async Task<int> GetCurrentSchoolIdAsync(CancellationToken cancellationToken)
        {
            if (_currentUserService.Role != UserRole.SchoolAdmin)
            {
                throw new UnauthorizedAccessException("Only school administrators can manage courses.");
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
    }
}
