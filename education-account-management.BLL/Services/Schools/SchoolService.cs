using DTOs.Base;
using DTOs.Schools;
using Interfaces.Audit;
using Interfaces.Schools;
using Services.Base;

namespace Services.Schools
{
    public class SchoolService(
        IUnitOfWork unitOfWork,
        SchoolMapper mapper,
        IManagementActionLogService managementActionLogService)
        : BaseService<School, CreateSchoolDTO, GetSchoolDTO, UpdateSchoolDTO>(
            unitOfWork,
            mapper),
            ISchoolService
    {
        private readonly IManagementActionLogService _managementActionLogService = managementActionLogService;

        public async Task UpdateSchoolsStatusAsync(BatchUpdateSchoolStatusDTO dto, CancellationToken cancellationToken)
        {
            var batchId = Guid.NewGuid();
            var schools = await _repository.GetByIdsAsync(dto.Ids, cancellationToken: cancellationToken);
            if (schools.Count != dto.Ids.Distinct().Count())
                throw new ValidationFailureException(nameof(dto.Ids), "One or more schools do not exist.");

            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                foreach (var school in schools)
                {
                    var oldStatus = school.Status;
                    var newStatus = (Enums.SchoolStatus)dto.Status;
                    school.Status = newStatus;
                    await _managementActionLogService.LogAsync(
                        batchId,
                        "School",
                        school.Id,
                        newStatus == Enums.SchoolStatus.Active ? "Activate" : "Deactivate",
                        dto.Reason,
                        oldStatus.ToString(),
                        newStatus.ToString(),
                        cancellationToken: token);
                }
                _repository.UpdateRange(schools);
            }, cancellationToken);
        }

        public override async Task DeleteSelectedIdsAsync(DeleteSelectedIdsDTO dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var batchId = Guid.NewGuid();
            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var schools = await _repository.GetTrackedByIdsAsync(dto.Ids, cancellationToken: token);
                if (schools.Count != dto.Ids.Distinct().Count())
                    throw new ValidationFailureException(nameof(dto.Ids), "One or more schools do not exist.");

                foreach (var school in schools)
                {
                    await _managementActionLogService.LogAsync(
                        batchId,
                        "School",
                        school.Id,
                        "Delete",
                        dto.Reason,
                        school.Status.ToString(),
                        null,
                        cancellationToken: token);
                }

                _repository.RemoveRange(schools);
            }, cancellationToken);
        }
    }
}
