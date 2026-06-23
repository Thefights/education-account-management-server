using Enums;
using Filters.Base;
using Models;

namespace DTOs.Courses
{
    public class StudentTuitionSummaryDTO
    {
        public decimal TotalOutstandingAmount { get; set; }
        public int UnpaidInvoicesCount { get; set; }
        public decimal EducationAccountBalance { get; set; }
    }

    public class StudentTuitionChargeDTO
    {
        public int EnrollmentId { get; set; }
        public int? ChargeId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string? CourseDescription { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string PaymentStatus { get; set; } = string.Empty; // "Paid", "Due", "Overdue"
        
        public decimal CourseFee { get; set; }
        public decimal MiscFee { get; set; }
        public decimal GstAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal FasSubsidyAmount { get; set; }
        public decimal NetPayable { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}
