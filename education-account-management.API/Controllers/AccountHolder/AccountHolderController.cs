using Authorization;
using Common.HttpResults;
using Controllers.Base;
using Interfaces.EducationAccounts;
using Filters.Courses;
using Interfaces.Courses;
using Interfaces.Payments;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.AccountHolder
{
    [Authorize(Roles = RolePolicy.AccountHolder)]
    public class AccountHolderController(
        IEducationAccountService educationAccountService,
        IStudentCourseService studentCourseService,
        IStudentTuitionService studentTuitionService) : BaseController
    {
        private readonly IEducationAccountService _educationAccountService = educationAccountService;
        private readonly IStudentCourseService _studentCourseService = studentCourseService;
        private readonly IStudentTuitionService _studentTuitionService = studentTuitionService;

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            var result = await _educationAccountService.GetAccountHolderProfileAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetMyCourses([FromQuery] StudentCourseFilterDTO filter, CancellationToken cancellationToken)
        {
            var result = await _studentCourseService.GetMyCoursesPaginatedAsync(filter, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("tuition-summary")]
        public async Task<IActionResult> GetTuitionSummary(CancellationToken cancellationToken)
        {
            var result = await _studentTuitionService.GetTuitionSummaryAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("tuition-charges")]
        public async Task<IActionResult> GetTuitionCharges([FromQuery] StudentTuitionFilterDTO filter, CancellationToken cancellationToken)
        {
            var result = await _studentTuitionService.GetTuitionChargesPaginatedAsync(filter, cancellationToken);
            return Result.SuccessData(result);
        }
    }
}