using DTOs.SchoolStudents;
using Interfaces.SchoolStudents;
using Mappers.SchoolStudents;
using Results;
using Services.Base;

namespace Services.SchoolStudents
{
    public class SchoolStudentService(
        IUnitOfWork unitOfWork,
        SchoolStudentMapper mapper,
        SchoolScopeResolver schoolScopeResolver)
        : BaseService<SchoolStudent, CreateSchoolStudentDTO, GetSchoolStudentDTO, UpdateSchoolStudentDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(SchoolStudent.EducationAccount), $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}"]),
            ISchoolStudentService
    {
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
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