using DTOs.AdminManagement;

namespace Mappers
{
    public class AdminManagementMapper : IReadMapper<User, GetAdminManagementDTO>
    {
        public GetAdminManagementDTO MapToGetDTO(User model)
        {
            return new GetAdminManagementDTO
            {
                UserId = model.Id,
                AuthAccountId = model.AuthAccountId,
                Role = model.Role,
                Status = model.AuthAccount.Status,
                AzureObjectId = model.AuthAccount.SsoIdentities
                    .Where(identity => identity.Provider == SsoProvider.AzureAD)
                    .Select(identity => identity.ProviderUserId)
                    .FirstOrDefault() ?? string.Empty,
                StaffCode = model.AdminProfile?.StaffCode ?? string.Empty,
                FullName = model.AdminProfile?.FullName ?? string.Empty,
                Email = model.AdminProfile?.Email ?? string.Empty,
                PhoneNumber = model.AdminProfile?.PhoneNumber,
                SchoolId = model.AdminProfile?.SchoolId,
                SchoolName = model.AdminProfile?.School?.SchoolName,
            };
        }

        public List<GetAdminManagementDTO> MapToGetDTOList(List<User> models)
        {
            return models.Select(MapToGetDTO).ToList();
        }

        public IQueryable<GetAdminManagementDTO> ProjectToGetDTO(IQueryable<User> query)
        {
            return query.Select(model => new GetAdminManagementDTO
            {
                UserId = model.Id,
                AuthAccountId = model.AuthAccountId,
                Role = model.Role,
                Status = model.AuthAccount.Status,
                AzureObjectId = model.AuthAccount.SsoIdentities
                    .Where(identity => identity.Provider == SsoProvider.AzureAD)
                    .Select(identity => identity.ProviderUserId)
                    .FirstOrDefault() ?? string.Empty,
                StaffCode = model.AdminProfile != null ? model.AdminProfile.StaffCode : string.Empty,
                FullName = model.AdminProfile != null ? model.AdminProfile.FullName : string.Empty,
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
