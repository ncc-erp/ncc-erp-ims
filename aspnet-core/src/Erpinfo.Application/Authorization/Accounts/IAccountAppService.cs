using System.Threading.Tasks;
using Abp.Application.Services;
using Erpinfo.Authorization.Accounts.Dto;

namespace Erpinfo.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
