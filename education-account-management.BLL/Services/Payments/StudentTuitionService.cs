using DTOs.Courses;
using Filters.Courses;
using Interfaces.Payments;
using Results;
using System.Linq.Expressions;

namespace Services.Payments
{
    public class StudentTuitionService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService) : IStudentTuitionService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<EducationAccount> _educationAccountRepository = unitOfWork.Repository<EducationAccount>();
        private readonly IGenericRepository<Enrollment> _enrollmentRepository = unitOfWork.Repository<Enrollment>();

        public async Task<StudentTuitionSummaryDTO> GetTuitionSummaryAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;
            var account = await _educationAccountRepository.Query()
                .Where(a => a.Citizen != null && a.Citizen.User != null && a.Citizen.User.Id == userId)
                .Select(a => new { a.Id, a.EducationCreditBalance })
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
            {
                throw new DataNotFoundException("Education account for the current account holder was not found.");
            }

            var chargesInfo = await _enrollmentRepository.Query()
                .Where(e => e.SchoolStudent.EducationAccountId == account.Id &&
                            e.Charge != null &&
                            e.Charge.Status != ChargeStatus.Paid &&
                            e.Charge.RemainingAmount > 0)
                .Select(e => e.Charge!.RemainingAmount)
                .ToListAsync(cancellationToken);

            return new StudentTuitionSummaryDTO
            {
                TotalOutstandingAmount = chargesInfo.Sum(),
                PendingPaymentInvoicesCount = chargesInfo.Count,
                EducationAccountBalance = account.EducationCreditBalance
            };
        }

        public async Task<PaginationResult<StudentTuitionChargeDTO>> GetTuitionChargesPaginatedAsync(
            StudentTuitionFilterDTO filter,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            var userId = _currentUserService.UserId;
            var accountId = await _educationAccountRepository.Query()
                .Where(a => a.Citizen != null && a.Citizen.User != null && a.Citizen.User.Id == userId)
                .Select(a => a.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (accountId == 0)
            {
                throw new DataNotFoundException("Education account for the current account holder was not found.");
            }

            var pageSize = Math.Clamp(filter.PageSize, 1, 100);

            Expression<Func<Enrollment, bool>> filterExpr = e =>
                e.SchoolStudent.EducationAccountId == accountId &&
                e.Charge != null &&
                (filter.IsInstallment == null || (filter.IsInstallment.Value ? e.Charge.Installments.Count > 1 : e.Charge.Installments.Count <= 1)) &&
                (filter.EnrollmentIds == null || filter.EnrollmentIds.Count == 0 || filter.EnrollmentIds.Contains(e.Id)) &&
                (filter.Status == StudentTuitionFilterStatus.All ||
                 (filter.Status == StudentTuitionFilterStatus.Paid && e.Charge.Status == ChargeStatus.Paid) ||
                 (filter.Status == StudentTuitionFilterStatus.Overdue && e.Charge.Status == ChargeStatus.Overdue) ||
                 (filter.Status == StudentTuitionFilterStatus.Due && e.Charge.Status == ChargeStatus.PendingPayment));

            var (total, charges) = await _enrollmentRepository.GetProjectedPaginatedAsync(
                projection: q => q.Select(e => new StudentTuitionChargeDTO
                {
                    EnrollmentId = e.Id,
                    ChargeId = e.Charge!.Id,
                    CourseCode = e.Course.CourseCode,
                    CourseName = e.CourseNameSnapshot,
                    CourseDescription = e.CourseDescriptionSnapshot,
                    PaymentDueDate = e.Charge.CourseEndDateSnapshot,
                    Status = e.Charge.Status,
                    CourseFee = e.Charge.CourseFeeAmountSnapshot,
                    MiscFee = e.Charge.MiscFeeAmountSnapshot,
                    GstAmount = e.Charge.GstAmountSnapshot,
                    GrossAmount = e.Charge.GrossAmount,
                    FasSubsidyAmount = e.Charge.SubsidyAmount,
                    NetPayable = e.Charge.NetAmount,
                    PaidAmount = e.Charge.PaidAmount,
                    RemainingAmount = e.Charge.RemainingAmount,
                    TaxRate = e.Charge.TaxRateSnapshot,
                    Installments = e.Charge.Installments.OrderBy(i => i.InstallmentNumber).Select(i => new StudentTuitionInstallmentDTO
                    {
                        Id = i.Id,
                        InstallmentNumber = i.InstallmentNumber,
                        Amount = i.Amount,
                        DueDate = i.DueDate,
                        Status = i.Status
                    }).ToList(),
                    AppliedFasSchemeName = e.Charge.AppliedFasSchemeNameSnapshot,
                    AppliedFasTierName = e.Charge.AppliedFasTierNameSnapshot
                }),
                filterExpr: filterExpr,
                filterStr: null,
                search: filter.Search,
                searchFields: [$"{nameof(Enrollment.Course)}.{nameof(Course.CourseCode)}", nameof(Enrollment.CourseNameSnapshot)],
                order: filter.SortExpression,
                page: filter.Page,
                pageSize: pageSize,
                includes: null,
                cancellationToken: cancellationToken);

            return new PaginationResult<StudentTuitionChargeDTO>(total, pageSize, charges);
        }
    }
}
