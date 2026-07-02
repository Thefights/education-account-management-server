using Authorization;
using Common.HttpResults;
using Controllers.Base;
using Interfaces.EducationAccounts;
using Filters.Courses;
using Interfaces.Courses;
using Interfaces.Payments;
using Filters.FasSchemes;
using Interfaces.FasSchemes;
using Interfaces.FasApplications;
using DTOs.FasApplications;
using DTOs.SupportTicket;
using Filters.SupportTickets;
using Interfaces.SupportTicket;

namespace Controllers.AccountHolder
{
    [Authorize(Roles = RolePolicy.AccountHolder)]
    public class AccountHolderController(
        IEducationAccountService educationAccountService,
        IStudentCourseService studentCourseService,
        IStudentTuitionService studentTuitionService,
        ISupportTicketService supportTicketService) : BaseController
    {
        private readonly IEducationAccountService _educationAccountService = educationAccountService;
        private readonly IStudentCourseService _studentCourseService = studentCourseService;
        private readonly IStudentTuitionService _studentTuitionService = studentTuitionService;
        private readonly ISupportTicketService _supportTicketService = supportTicketService;

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

        [HttpGet("courses/{id:int}")]
        public async Task<IActionResult> GetMyCourseDetail(int id, CancellationToken cancellationToken)
        {
            var result = await _studentCourseService.GetMyCourseDetailAsync(id, cancellationToken);
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

        [HttpPost("support-tickets")]
        public async Task<IActionResult> CreateTicket(CreateSupportTicketDTO request, CancellationToken cancellationToken)
        {
            await _supportTicketService.CreateTicketAsync(request, cancellationToken);
            return Result.SuccessAction("Ticket created successfully");
        }

        [HttpGet("support-tickets")]
        public async Task<IActionResult> GetMyTickets([FromQuery] SupportTicketFilterDTO filterDTO, CancellationToken cancellationToken)
        {
            var tickets = await _supportTicketService.GetMyTicketsAsync(filterDTO, cancellationToken);
            return Result.SuccessData(tickets);
        }
    }
}
