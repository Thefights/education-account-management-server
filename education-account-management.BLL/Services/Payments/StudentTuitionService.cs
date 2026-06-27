using DTOs.Courses;
using Enums;
using Exceptions;
using Filters.Courses;
using Infrastructure.Interface;
using Interfaces.Payments;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Payments
{
    public class StudentTuitionService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService) : IStudentTuitionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
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

            TimeZoneInfo sgtZone;
            try
            {
                sgtZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Singapore");
            }
            catch (TimeZoneNotFoundException)
            {
                sgtZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            }
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, sgtZone);
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);

            Expression<Func<Enrollment, bool>> filterExpr = e =>
                e.SchoolStudent.EducationAccountId == accountId &&
                e.Charge != null &&
                (filter.EnrollmentIds == null || filter.EnrollmentIds.Count == 0 || filter.EnrollmentIds.Contains(e.Id)) &&
                (filter.Status == StudentTuitionFilterStatus.All ||
                 (filter.Status == StudentTuitionFilterStatus.Paid && e.Charge.Status == ChargeStatus.Paid) ||
                 (filter.Status == StudentTuitionFilterStatus.Overdue && 
                     (e.Charge.Status == ChargeStatus.Overdue || 
                      (e.Charge.Status == ChargeStatus.PendingPayment && e.Course.FasApplicationDueDate < now))) ||
                 (filter.Status == StudentTuitionFilterStatus.Due && 
                     e.Charge.Status == ChargeStatus.PendingPayment && 
                     e.Course.FasApplicationDueDate >= now));

            var (total, charges) = await _enrollmentRepository.GetProjectedPaginatedAsync(
                projection: q => q.Select(e => new StudentTuitionChargeDTO
                {
                    EnrollmentId = e.Id,
                    ChargeId = e.Charge!.Id,
                    CourseCode = e.Course.CourseCode,
                    CourseName = e.CourseNameSnapshot,
                    CourseDescription = e.CourseDescriptionSnapshot,
                    PaymentDueDate = e.Course.FasApplicationDueDate,
                    PaymentStatus = e.Charge.Status == ChargeStatus.Paid ? "Paid" :
                                    e.Charge.Installments.Count > 1 ? "Installment" :
                                    (e.Charge.Status == ChargeStatus.Overdue || e.Course.FasApplicationDueDate < now) ? "Overdue" :
                                    e.Charge.Installments.Count > 1 ? "Installment" :
                                    "Due",
                    CourseFee = e.Charge.CourseFeeAmountSnapshot,
                    MiscFee = e.Charge.MiscFeeAmountSnapshot,
                    GstAmount = e.Charge.GstAmountSnapshot,
                    GrossAmount = e.Charge.GrossAmount,
                    FasSubsidyAmount = e.Charge.SubsidyAmount,
                    NetPayable = e.Charge.NetAmount,
                    PaidAmount = e.Charge.PaidAmount,
                    RemainingAmount = e.Charge.RemainingAmount,
                    TaxRate = e.Charge.TaxRateSnapshot,
                    IsInstallment = e.Charge.Installments.Count > 1,
                    CurrentInstallmentNumber = e.Charge.Installments.Count > 1 ? e.Charge.Installments.Where(i => i.Status != ChargeInstallmentStatus.Paid).OrderBy(i => i.InstallmentNumber).Select(i => (int?)i.InstallmentNumber).FirstOrDefault() : null,
                    TotalInstallments = e.Charge.Installments.Count > 1 ? (int?)e.Charge.Installments.Count : null,
                    AppliedFasSchemeName = e.Charge.AppliedFasSchemeNameSnapshot,
                    AppliedFasTierName = e.Charge.AppliedFasTierNameSnapshot,
                    HasFasApplication = e.Charge.AppliedFasApplicationId.HasValue
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
