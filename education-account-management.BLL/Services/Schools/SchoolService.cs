using DTOs.Schools;
using Interfaces.Schools;
using Services.Base;

namespace Services.Schools
{
    public class SchoolService(
        IUnitOfWork unitOfWork,
        SchoolMapper mapper)
        : BaseService<School, CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO>(
            unitOfWork,
            mapper),
            ISchoolService
    {
        public async Task UpdateSchoolsStatusAsync(BatchUpdateSchoolStatusDTO dto, CancellationToken cancellationToken)
        {
            var schools = await _repository.GetByIdsAsync(dto.Ids, cancellationToken: cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                foreach (var school in schools)
                {
                    school.Status = (Enums.SchoolStatus)dto.Status;
                }
                _repository.UpdateRange(schools);
            }, cancellationToken);
        }
    }
}
