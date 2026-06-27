using Enums;
using Models;

namespace DTOs.FasApplications
{
    public class SubmitFasApplicationDocumentDTO
    {
        [NotDefaultValue]
        public int RequiredDocumentId { get; set; }

        public string FileKey { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
    }

    public class SubmitFasApplicationDTO
    {
        [NotDefaultValue]
        public int FasSchemeId { get; set; }

        public int? ReapplySourceApplicationId { get; set; }

        [EnumDefined]
        public NationalityCategory GuardianNationality { get; set; }

        public decimal GrossHouseholdIncome { get; set; }

        public int HouseholdMemberCount { get; set; }

        public List<SubmitFasApplicationDocumentDTO> Documents { get; set; } = [];
    }
}
