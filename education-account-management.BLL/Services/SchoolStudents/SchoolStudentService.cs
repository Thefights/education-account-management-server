using DTOs.Base;
using DTOs.SchoolStudents;
using Interfaces.Audit;
using Interfaces.SchoolStudents;
using Mappers.SchoolStudents;
using Results;
using Services.Base;

namespace Services.SchoolStudents
{
    public class SchoolStudentService(
        IUnitOfWork unitOfWork,
        SchoolStudentMapper mapper,
        SchoolScopeResolver schoolScopeResolver,
        IManagementActionLogService managementActionLogService)
        : BaseService<SchoolStudent, CreateSchoolStudentDTO, GetSchoolStudentDTO, UpdateSchoolStudentDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(SchoolStudent.EducationAccount), $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}"]),
            ISchoolStudentService
    {
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly IManagementActionLogService _managementActionLogService = managementActionLogService;
        private readonly IGenericRepository<Citizen> _citizenRepository = unitOfWork.Repository<Citizen>();
        private readonly IGenericRepository<EducationAccount> _educationAccountRepository = unitOfWork.Repository<EducationAccount>();

        public override async Task<GetSchoolStudentDTO> CreateAsync(CreateSchoolStudentDTO createDTO, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var citizen = await _citizenRepository.Query().FirstOrDefaultAsync(c => c.Nric == createDTO.Nric, cancellationToken)
                ?? throw new DataNotFoundException("Student with this NRIC not found.");

            var eduAccount = await _educationAccountRepository.Query().FirstOrDefaultAsync(e => e.CitizenId == citizen.Id, cancellationToken)
                ?? throw new DataNotFoundException("This student does not have an Education Account.");

            var existingSchoolStudent = await _repository.Query().FirstOrDefaultAsync(ss => ss.EducationAccountId == eduAccount.Id, cancellationToken);
            if (existingSchoolStudent != null)
            {
                if (existingSchoolStudent.SchoolId == schoolId)
                {
                    throw new DataConflictException("This student is already in your school.");
                }
                else
                {
                    throw new DataConflictException("This student belongs to another school.");
                }
            }

            var entityId = await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                var newSchoolStudent = new SchoolStudent
                {
                    SchoolId = schoolId,
                    EducationAccountId = eduAccount.Id,
                };

                var addedEntity = await _repository.AddAsync(newSchoolStudent, token);
                await _unitOfWork.SaveChangeAsync(token);
                return addedEntity.Id;
            }, cancellationToken);

            return await GetByIdAsync(entityId, cancellationToken);
        }

        public override async Task<GetSchoolStudentDTO> UpdateAsync(int id, UpdateSchoolStudentDTO updateDTO, CancellationToken cancellationToken = default)
        {
            var batchId = Guid.NewGuid();
            var ids = updateDTO.ListIds.Count > 0 ? updateDTO.ListIds : [id];
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var students = await _repository.Query(tracking: true)
                    .Where(student => ids.Contains(student.Id) && student.SchoolId == schoolId)
                    .ToListAsync(token);
                if (students.Count != ids.Distinct().Count())
                    throw new ValidationFailureException(nameof(updateDTO.ListIds), "One or more students do not exist or you do not have permission.");

                foreach (var student in students)
                {
                    var oldStatus = student.Status;
                    var newStatus = (SchoolStudentStatus)updateDTO.Status;
                    student.Status = newStatus;
                    student.TryValidate();
                    await _managementActionLogService.LogAsync(
                        batchId,
                        ManagementActionEntityType.SchoolStudent,
                        student.Id,
                        newStatus == SchoolStudentStatus.Active ? ManagementAction.Activate : ManagementAction.Deactivate,
                        updateDTO.Reason,
                        oldStatus.ToString(),
                        newStatus.ToString(),
                        cancellationToken: token);
                }

                _repository.UpdateRange(students);
            }, cancellationToken);

            return await GetByIdAsync(id, cancellationToken);
        }

        public override async Task DeleteSelectedIdsAsync(DeleteSelectedIdsDTO dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var reason = dto.Reason;
            var batchId = Guid.NewGuid();
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var ids = dto.Ids.Distinct().ToList();
                var students = await _repository.Query(tracking: true)
                    .Where(item => ids.Contains(item.Id) && item.SchoolId == schoolId)
                    .ToListAsync(token);
                if (students.Count != ids.Count)
                    throw new ValidationFailureException(nameof(dto.Ids), "One or more students do not exist or you do not have permission.");

                foreach (var student in students)
                {
                    await _managementActionLogService.LogAsync(
                        batchId,
                        ManagementActionEntityType.SchoolStudent,
                        student.Id,
                        ManagementAction.Delete,
                        reason,
                        student.Status.ToString(),
                        null,
                        cancellationToken: token);
                }

                _repository.RemoveRange(students);
            }, cancellationToken);
        }

        public override async Task<PaginationResult<GetSchoolStudentDTO>> GetAllPaginatedAsync(FilterDTO filterDTO, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);
            var (total, students) = await _repository.GetProjectedPaginatedAsync(
                _mapper.ProjectToGetDTO,
                ss => ss.SchoolId == schoolId,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression,
                filterDTO.Page,
                pageSize,
                _includes,
                cancellationToken);

            return new PaginationResult<GetSchoolStudentDTO>(total, pageSize, students);
        }
    }
}
