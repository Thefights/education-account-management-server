using DTOs.Courses;
using Enums;
using Exceptions;
using Filters.Courses;
using Infrastructure.Interface;
using Interfaces.Base;
using Interfaces.Courses;
using Mappers;
using Microsoft.EntityFrameworkCore;
using Models;
using Results;
using System.Linq.Expressions;

namespace Services.Courses
{
    public class StudentCourseService(
        IUnitOfWork unitOfWork,
        CourseMapper mapper,
        ICurrentUserService currentUserService) : IStudentCourseService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly CourseMapper _mapper = mapper;
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
                projection: query => _mapper.ProjectToGetDTO(query.Select(e => e.Course)),
                filterExpr: e => e.SchoolStudent.EducationAccountId == accountId 
                              && e.Course.Status != CourseStatus.Draft 
                              && e.Course.Status != CourseStatus.Enrolling 
                              && e.Course.Status == filter.Tab,
                filterStr: null,
                search: null,
                searchFields: null,
                order: filter.SortExpression,
                page: filter.Page,
                pageSize: pageSize,
                includes: null,
                cancellationToken: cancellationToken);

            return new PaginationResult<GetCourseDTO>(total, pageSize, courses);
        }
    }
}
