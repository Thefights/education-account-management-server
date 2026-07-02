using DTOs.Courses;
using Enums;
using Exceptions;
using Filters.Courses;
using Infrastructure.Interface;
using Interfaces.Base;
using Interfaces.Courses;
using Microsoft.EntityFrameworkCore;
using Models;
using Results;
using System.Linq.Expressions;

namespace Services.Courses
{
    public class StudentCourseService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService) : IStudentCourseService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<EducationAccount> _educationAccountRepository = unitOfWork.Repository<EducationAccount>();
        private readonly IGenericRepository<Enrollment> _enrollmentRepository = unitOfWork.Repository<Enrollment>();

        public async Task<PaginationResult<GetCourseDTO>> GetMyCoursesPaginatedAsync(
    StudentCourseFilterDTO filter,
    CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            if (filter.Tab == CourseStatus.Draft || filter.Tab == CourseStatus.Enrolling)
            {
                throw new ValidationFailureException(
                    nameof(filter.Tab),
                    "Students are not allowed to view courses in Draft or Enrolling status.");
            }

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

            //Default StartDate Desc
            var (total, courses) = await _enrollmentRepository.GetProjectedPaginatedAsync(
                projection: query => query.Select(e => new GetCourseDTO
                {
                    Id = e.Course.Id,
                    SchoolId = e.Course.SchoolId,
                    CreatedAt = e.Course.CreatedAt,
                    SchoolName = e.Course.School.SchoolName,
                    Status = e.Course.Status.ToString(),
                    CourseCode = e.Course.CourseCode,
                    CourseName = e.Course.CourseName,
                    CourseFeeAmount = e.Course.CourseFeeAmount,
                    MiscFeeAmount = e.Course.MiscFeeAmount,
                    GstAmount = e.Course.GstAmount,
                    EnrollmentDeadline = e.Course.EnrollmentDeadline,
                    StartDate = e.Course.StartDate,
                    EndDate = e.Course.EndDate,
                    EnrollmentCount = e.Course.Enrollments.Count,
                    ActiveEnrollmentCount = e.Course.Enrollments.Count(item => item.Status == EnrollmentStatus.Active),
                    WithdrawnEnrollmentCount = e.Course.Enrollments.Count(item => item.Status == EnrollmentStatus.Withdrawn),
                    EnrollmentStatus = e.Status.ToString(),
                    ApplicableFasSchemes = e.Course.FasSchemeCourses
                        .OrderBy(item => item.FasScheme.SchemeName)
                        .Select(item => new GetCourseFasSchemeDTO
                        {
                            Id = item.FasScheme.Id,
                            SchemeCode = item.FasScheme.SchemeCode,
                            SchemeName = item.FasScheme.SchemeName,
                            Status = item.FasScheme.Status.ToString()
                        })
                        .ToList()
                }),
                filterExpr: e => e.SchoolStudent.EducationAccountId == accountId
                              && e.Course.Status != CourseStatus.Draft
                              && e.Course.Status != CourseStatus.Enrolling
                              && e.Course.Status == filter.Tab,
                filterStr: filter.Filter,
                search: filter.Search,
                searchFields: filter.SearchFields,
                order: filter.SortExpression,
                page: filter.Page,
                pageSize: pageSize,
                includes: null,
                cancellationToken: cancellationToken);

            return new PaginationResult<GetCourseDTO>(total, pageSize, courses);
        }

        public async Task<StudentCourseDetailDTO> GetMyCourseDetailAsync(
            int courseId,
            CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;
            var accountId = await _educationAccountRepository.Query()
                .Where(a => a.Citizen != null && a.Citizen.User != null && a.Citizen.User.Id == userId)
                .Select(a => a.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (accountId == 0)
            {
                throw new DataNotFoundException("Education account for the current account holder was not found.");
            }

            var detail = await _enrollmentRepository.Query()
                .Where(e => e.SchoolStudent.EducationAccountId == accountId
                            && e.CourseId == courseId
                            && e.Course.Status != CourseStatus.Draft
                            && e.Course.Status != CourseStatus.Enrolling)
                .Select(e => new StudentCourseDetailDTO
                {
                    Id = e.Course.Id,
                    CourseCode = e.Course.CourseCode,
                    CourseName = e.Course.CourseName,
                    Status = e.Course.Status.ToString(),
                    EnrollmentDeadline = e.Course.EnrollmentDeadline,
                    StartDate = e.Course.StartDate,
                    EndDate = e.Course.EndDate,
                    CourseFeeAmount = e.Charge != null
                        ? e.Charge.CourseFeeAmountSnapshot
                        : e.Course.CourseFeeAmount,
                    MiscFeeAmount = e.Charge != null
                        ? e.Charge.MiscFeeAmountSnapshot
                        : e.Course.MiscFeeAmount,
                    GstAmount = e.Charge != null
                        ? e.Charge.GstAmountSnapshot
                        : e.Course.GstAmount,
                    GrossAmount = e.Charge != null
                        ? e.Charge.GrossAmount
                        : e.Course.CourseFeeAmount + e.Course.MiscFeeAmount + e.Course.GstAmount,
                    FasDeductionAmount = e.Charge != null ? e.Charge.SubsidyAmount : 0m,
                    TotalToPay = e.Charge != null
                        ? e.Charge.NetAmount
                        : e.Course.CourseFeeAmount + e.Course.MiscFeeAmount + e.Course.GstAmount,
                    AppliedFasSchemeName = e.Charge != null
                        ? e.Charge.AppliedFasSchemeNameSnapshot
                        : null,
                    AppliedFasTierName = e.Charge != null
                        ? e.Charge.AppliedFasTierNameSnapshot
                        : null,
                    ApplicableFasSchemes = e.Course.FasSchemeCourses
                        .OrderBy(item => item.FasScheme.SchemeName)
                        .Select(item => new GetCourseFasSchemeDTO
                        {
                            Id = item.FasScheme.Id,
                            SchemeCode = item.FasScheme.SchemeCode,
                            SchemeName = item.FasScheme.SchemeName,
                            Status = item.FasScheme.Status.ToString()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return detail ?? throw new DataNotFoundException("Course was not found for the current account holder.");
        }
    }
}
