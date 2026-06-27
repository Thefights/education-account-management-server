using DTOs.Admin;

namespace Mappers.Admin
{
    public class AdminMapper : IReadMapper<User, GetAdminDTO>
    {
        public GetAdminDTO MapToGetDTO(User model)
        {
            return new GetAdminDTO
            {
                UserId = model.Id,
                CreatedAt = model.CreatedAt,
                Role = model.Role.ToString(),
                Status = model.Status.ToString(),
                AzureObjectId = model.SsoIdentities
                    .Where(identity => identity.Provider == SsoProvider.AzureAD)
                    .Select(identity => identity.ProviderUserId)
                    .FirstOrDefault() ?? string.Empty,
                StaffCode = model.AdminProfile?.StaffCode ?? string.Empty,
                FullName = model.AdminProfile?.FullName ?? string.Empty,
                Nric = model.AdminProfile?.Nric ?? string.Empty,
                Email = model.AdminProfile?.Email ?? string.Empty,
                PhoneNumber = model.AdminProfile?.PhoneNumber,
                SchoolId = model.AdminProfile?.SchoolId,
                SchoolName = model.AdminProfile?.School?.SchoolName,
            };
        }

        public List<GetAdminDTO> MapToGetDTOList(List<User> models)
        {
            return models.Select(MapToGetDTO).ToList();
        }

        public IQueryable<GetAdminDTO> ProjectToGetDTO(IQueryable<User> query)
        {
            return query.Select(model => new GetAdminDTO
            {
                UserId = model.Id,
                CreatedAt = model.CreatedAt,
                Role = model.Role.ToString(),
                Status = model.Status.ToString(),
                AzureObjectId = model.SsoIdentities
                    .Where(identity => identity.Provider == SsoProvider.AzureAD)
                    .Select(identity => identity.ProviderUserId)
                    .FirstOrDefault() ?? string.Empty,
                StaffCode = model.AdminProfile != null ? model.AdminProfile.StaffCode : string.Empty,
                FullName = model.AdminProfile != null ? model.AdminProfile.FullName : string.Empty,
                Nric = model.AdminProfile != null ? model.AdminProfile.Nric : string.Empty,
                Email = model.AdminProfile != null ? model.AdminProfile.Email : string.Empty,
                PhoneNumber = model.AdminProfile != null ? model.AdminProfile.PhoneNumber : null,
                SchoolId = model.AdminProfile != null ? model.AdminProfile.SchoolId : null,
                SchoolName = model.AdminProfile != null && model.AdminProfile.School != null
                    ? model.AdminProfile.School.SchoolName
                    : null,
            });
        }
    }
}
