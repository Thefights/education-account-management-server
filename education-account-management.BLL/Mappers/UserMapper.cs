using DTOs.User;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class UserMapper : ICrudMapper<User, CreateUserDTO, GetUserDTO, UpdateUserDTO>
    {
        [MapProperty("AuthAccount.UserIdText", nameof(GetUserDTO.UserIdText))]
        [MapProperty("AuthAccount.Email", nameof(GetUserDTO.Email))]
        [MapProperty("AuthAccount.Status", nameof(GetUserDTO.Status))]
        [MapProperty("AuthAccount.FailedLoginCount", nameof(GetUserDTO.FailedLoginCount))]
        [MapProperty("AuthAccount.LockedUntil", nameof(GetUserDTO.LockedUntil))]
        [MapProperty("AuthAccount.LastLoginAt", nameof(GetUserDTO.LastLoginAt))]
        public partial GetUserDTO MapToGetDTO(User model);

        public partial List<GetUserDTO> MapToGetDTOList(List<User> models);

        [MapperIgnoreSource(nameof(CreateUserDTO.ImageUrl))]
        public partial User MapFromCreateDTO(CreateUserDTO createDTO);

        [MapperIgnoreSource(nameof(UpdateUserDTO.ImageUrl))]
        public partial void MapFromUpdateDTO(UpdateUserDTO updateDTO, User model);

        public partial IQueryable<GetUserDTO> ProjectToGetDTO(IQueryable<User> query);
    }
}
