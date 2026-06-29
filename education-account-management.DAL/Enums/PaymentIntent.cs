
namespace Enums
{
    public enum PaymentIntent
    {
        PayFull = 1,                 // Trả thẳng toàn bộ (Không trả góp)
        CreateInstallment = 2,       // Tạo mới trả góp (Dùng kèm InstallmentNumber)
        PayCurrentInstallment = 3,   // Trả tháng hiện tại của trả góp đang có
        PayRemainingInstallments = 4 // Tất toán toàn bộ số tháng trả góp còn nợ
    }
}
