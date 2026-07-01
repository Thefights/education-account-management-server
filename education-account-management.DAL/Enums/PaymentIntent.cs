namespace Enums
{
    public enum PaymentIntent
    {
        PayFull = 1,                 // Trả thẳng toàn bộ (Không trả góp)
        CreateInstallment = 2,       // Tạo mới trả góp (Dùng kèm InstallmentNumber)
        PayDueInstallments = 3,      // Trả một hoặc nhiều kỳ đã đến hạn theo thứ tự cũ nhất
        PayRemainingInstallments = 4 // Tất toán toàn bộ số tháng trả góp còn nợ
    }
}
