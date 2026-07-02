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

            var page = Math.Max(filter.Page, 1);
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);
            var statuses = filter.Statuses?
                .Where(status => status != StudentTuitionFilterStatus.All)
                .Distinct()
                .ToList() ?? [];

            var utcToday = DateTime.UtcNow.Date;
            var payableThrough = PaymentDueWindow.GetPayableThrough(utcToday);
            var hasRequestedEnrollmentIds = filter.EnrollmentIds != null && filter.EnrollmentIds.Count > 0;

            Expression<Func<Enrollment, bool>> filterExpr = e =>
                e.SchoolStudent.EducationAccountId == accountId &&
                e.Charge != null &&
                (filter.IsInstallment == null ||
                    (filter.IsInstallment.Value
                        ? e.Charge.Installments.Count > 1 &&
                          (hasRequestedEnrollmentIds ||
                           e.Charge.Installments.Any(i =>
                               i.Status != ChargeInstallmentStatus.Paid &&
                               (i.Status == ChargeInstallmentStatus.Overdue || i.DueDate.Date <= payableThrough)))
                        : e.Charge.Installments.Count <= 1 &&
                          (hasRequestedEnrollmentIds ||
                           e.Charge.Status == ChargeStatus.Paid ||
                           e.Charge.Status == ChargeStatus.Overdue ||
                           e.Charge.CourseEndDateSnapshot.Date <= payableThrough))) &&
                (filter.EnrollmentIds == null || filter.EnrollmentIds.Count == 0 || filter.EnrollmentIds.Contains(e.Id)) &&
                (statuses.Count == 0 ||
                 (statuses.Contains(StudentTuitionFilterStatus.Paid) && e.Charge.Status == ChargeStatus.Paid) ||
                 (statuses.Contains(StudentTuitionFilterStatus.Overdue) && e.Charge.Status == ChargeStatus.Overdue) ||
                 (statuses.Contains(StudentTuitionFilterStatus.Due) &&
                  e.Charge.Status == ChargeStatus.PendingPayment &&
                  (filter.IsInstallment == true
                    ? e.Charge.Installments.Any(i =>
                        i.Status != ChargeInstallmentStatus.Paid &&
                        i.DueDate.Date <= payableThrough)
                    : e.Charge.CourseEndDateSnapshot.Date <= payableThrough)));

            _ = filter.SortExpression;

            var query = _enrollmentRepository.Query()
                .Where(filterExpr);

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim().ToLowerInvariant();
                query = query.Where(e =>
                    e.Course.CourseCode.ToLower().Contains(search) ||
                    e.CourseNameSnapshot.ToLower().Contains(search));
            }

            var total = await query.CountAsync(cancellationToken);
            query = ApplyTuitionOrdering(query, filter)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var charges = await query
                .Select(e => new StudentTuitionChargeDTO
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
                    Installments = e.Charge.Installments
                        .OrderBy(i => i.Status == ChargeInstallmentStatus.Overdue ? 0 :
                                      i.Status == ChargeInstallmentStatus.PendingPayment ? 1 :
                                      i.Status == ChargeInstallmentStatus.Paid ? 2 : 3)
                        .ThenBy(i => i.InstallmentNumber)
                        .Select(i => new StudentTuitionInstallmentDTO
                    {
                        Id = i.Id,
                        InstallmentNumber = i.InstallmentNumber,
                        Amount = i.Amount,
                        DueDate = i.DueDate,
                        Status = i.Status
                    }).ToList(),
                    AppliedFasSchemeName = e.Charge.AppliedFasSchemeNameSnapshot,
                    AppliedFasTierName = e.Charge.AppliedFasTierNameSnapshot
                })
                .ToListAsync(cancellationToken);

            return new PaginationResult<StudentTuitionChargeDTO>(total, pageSize, charges);
        }

        private static IOrderedQueryable<Enrollment> ApplyTuitionOrdering(
            IQueryable<Enrollment> query,
            StudentTuitionFilterDTO filter)
        {
            var ordered = query
                .OrderBy(e => e.Charge!.Status == ChargeStatus.Overdue ? 0 :
                              e.Charge.Status == ChargeStatus.PendingPayment ? 1 :
                              e.Charge.Status == ChargeStatus.Paid ? 2 : 3);

            var firstSortTerm = (filter.Sort ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .FirstOrDefault() ?? string.Empty;
            var tokens = firstSortTerm
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var alias = tokens.Length > 0 ? tokens[0] : "id";
            var descending = tokens.Length < 2 ||
                             string.Equals(tokens[1], "desc", StringComparison.OrdinalIgnoreCase);

            return alias.ToLowerInvariant() switch
            {
                "createdat" => descending
                    ? ordered.ThenByDescending(e => e.Charge!.CreatedAt).ThenByDescending(e => e.Id)
                    : ordered.ThenBy(e => e.Charge!.CreatedAt).ThenBy(e => e.Id),
                _ => descending
                    ? ordered.ThenByDescending(e => e.Id)
                    : ordered.ThenBy(e => e.Id)
            };
        }
    }
}
