using DTOs.Csv;
using DTOs.EducationAccounts;
using Interfaces.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Interfaces.EducationAccounts
{
    public interface IEducationAccountService :
        IBaseCrudService<CreateEducationAccountDTO, GetEducationAccountDTO, UpdateEducationAccountDTO>
    {
        Task<GetEducationAccountDTO> GetAccountHolderProfileAsync(
            CancellationToken cancellationToken = default);
    }
}
