using Abp.Application.Services;
using Erpinfo.MultiTenancy.Dto;

namespace Erpinfo.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

