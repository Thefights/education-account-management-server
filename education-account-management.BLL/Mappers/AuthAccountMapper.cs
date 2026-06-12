using DTOs.Auth;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class AuthAccountMapper : ICrudMapper<AuthAccount, CreateAuthAccountDTO, GetAuthAccountDTO, UpdateAuthAccountDTO>
    {
        public GetAuthAccountDTO MapToGetDTO(AuthAccount model)
        {
            return new GetAuthAccountDTO
            {
                Id = model.Id,
                UserId = model.User.Id,
                UserIdText = model.UserIdText,
                Email = model.Email,
                Status = model.Status.ToString(),
                FullName = model.User.FullName,
                PhoneNumber = model.User.PhoneNumber,
                Gender = model.User.Gender.ToString(),
                ImageUrl = model.User.ImageUrl,
                RoleIds = model.User.UserRoles.Select(userRole => userRole.RoleId).ToList()
            };
        }

        [MapProperty("User.FullName", nameof(GetAuthAccountProfileDTO.FullName))]
        [MapProperty("User.PhoneNumber", nameof(GetAuthAccountProfileDTO.PhoneNumber))]
        [MapProperty("User.Gender", nameof(GetAuthAccountProfileDTO.Gender))]
        [MapProperty("User.ImageUrl", nameof(GetAuthAccountProfileDTO.ImageUrl))]
        public partial GetAuthAccountProfileDTO MapToGetProfileDTO(AuthAccount model);

        public List<GetAuthAccountDTO> MapToGetDTOList(List<AuthAccount> models)
        {
            return models.Select(MapToGetDTO).ToList();
        }

        [MapperIgnoreSource(nameof(CreateAuthAccountDTO.FullName))]
        [MapperIgnoreSource(nameof(CreateAuthAccountDTO.PhoneNumber))]
        [MapperIgnoreSource(nameof(CreateAuthAccountDTO.Gender))]
        [MapperIgnoreSource(nameof(CreateAuthAccountDTO.ImageUrl))]
        [MapperIgnoreSource(nameof(CreateAuthAccountDTO.RoleIds))]
        public partial AuthAccount MapFromCreateDTO(CreateAuthAccountDTO createDTO);

        [MapperIgnoreSource(nameof(UpdateAuthAccountDTO.FullName))]
        [MapperIgnoreSource(nameof(UpdateAuthAccountDTO.PhoneNumber))]
        [MapperIgnoreSource(nameof(UpdateAuthAccountDTO.Gender))]
        [MapperIgnoreSource(nameof(UpdateAuthAccountDTO.ImageUrl))]
        [MapperIgnoreSource(nameof(UpdateAuthAccountDTO.RoleIds))]
        public partial void MapFromUpdateDTO(UpdateAuthAccountDTO updateDTO, AuthAccount model);

        [MapProperty(nameof(UpdateAuthAccountProfileDTO.FullName), "User.FullName")]
        [MapProperty(nameof(UpdateAuthAccountProfileDTO.PhoneNumber), "User.PhoneNumber")]
        [MapProperty(nameof(UpdateAuthAccountProfileDTO.Gender), "User.Gender")]
        [MapperIgnoreSource(nameof(UpdateAuthAccountProfileDTO.ImageUrl))]
        public partial void MapFromUpdateProfileDTO(UpdateAuthAccountProfileDTO updateDTO, AuthAccount model);

        public IQueryable<GetAuthAccountDTO> ProjectToGetDTO(IQueryable<AuthAccount> query)
        {
            return query.Select(model => new GetAuthAccountDTO
            {
                Id = model.Id,
                UserId = model.User.Id,
                UserIdText = model.UserIdText,
                Email = model.Email,
                Status = model.Status.ToString(),
                FullName = model.User.FullName,
                PhoneNumber = model.User.PhoneNumber,
                Gender = model.User.Gender.ToString(),
                ImageUrl = model.User.ImageUrl,
                RoleIds = model.User.UserRoles.Select(userRole => userRole.RoleId).ToList()
            });
        }

        public partial IQueryable<GetAuthAccountProfileDTO> ProjectToGetProfileDTO(IQueryable<AuthAccount> query);
    }
}
