using DTOs.TransactionHistory;
using Mappers.Base;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class TransactionHistoryMapper
        : IReadMapper<EducationCreditTransaction, EducationCreditTransactionDTO>
    {
        [MapProperty($"{nameof(EducationCreditTransaction.Payment)}.{nameof(Payment.PaymentMethod)}",
            nameof(EducationCreditTransactionDTO.PaymentMethod))]
        public partial EducationCreditTransactionDTO MapToGetDTO(EducationCreditTransaction model);

        public partial List<EducationCreditTransactionDTO> MapToGetDTOList(
            List<EducationCreditTransaction> models);

        public partial IQueryable<EducationCreditTransactionDTO> ProjectToGetDTO(
            IQueryable<EducationCreditTransaction> query);
    }
}
