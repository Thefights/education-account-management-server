using EntityAnnotations;
using Enums;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Admin
{
    public class UpdateAdminStatusDTO
    {
        [MessageRequired]
        public List<int> AdminIds { get; set; } = [];

        [EnumDefined]
        public UserStatus Status { get; set; }

        [MessageRequired]
        [MessageMinLength(10)]
        public string Reason { get; set; } = string.Empty;
    }
}
