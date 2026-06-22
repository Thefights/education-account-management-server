using DTOs.EducationAccounts;
using Models;
using Riok.Mapperly.Abstractions;

namespace Mappers.EducationAccounts
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public static partial class EducationAccountSweepReportMapper
    {
        public static partial EducationAccountSweepResultDTO MapToResultDTO(EducationAccountSweepReport report);
    }
}
