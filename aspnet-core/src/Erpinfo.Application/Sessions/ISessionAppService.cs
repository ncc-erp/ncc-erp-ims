using System.Threading.Tasks;
using Abp.Application.Services;
using Erpinfo.Sessions.Dto;

namespace Erpinfo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
