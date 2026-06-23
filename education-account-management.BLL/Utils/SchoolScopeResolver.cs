namespace Utils;

public class SchoolScopeResolver(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
{
    private readonly IGenericRepository<AdminProfile> _adminProfileRepository = unitOfWork.Repository<AdminProfile>();
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private int? _schoolId;

    public async Task<int> GetSchoolIdAsync(
        CancellationToken cancellationToken = default)
    {
        if (_currentUserService.Role != UserRole.SchoolAdmin)
        {
            throw new UnauthorizedAccessException(
                "Only school administrators can perform this action.");
        }

        if (_schoolId.HasValue)
        {
            return _schoolId.Value;
        }

        var schoolId = await _adminProfileRepository.Query()
            .Where(profile => profile.UserId == _currentUserService.UserId)
            .Select(profile => profile.SchoolId)
            .SingleOrDefaultAsync(cancellationToken);

        _schoolId = schoolId
            ?? throw new UnauthorizedAccessException(
                "The school administrator is not assigned to a school.");

        return _schoolId.Value;
    }
}