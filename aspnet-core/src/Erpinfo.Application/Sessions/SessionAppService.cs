using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using Erpinfo.Authorization.Roles;
using Erpinfo.Sessions.Dto;

namespace Erpinfo.Sessions
{
    public class SessionAppService : ErpinfoAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>(),

                },
            };
            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
                output.User.Role = ErpSession.Role;
                output.User.Avatar = !string.IsNullOrEmpty(output.User.Avatar) ? output.User.Avatar : "";
            }
            if (!ErpSession.GroupId.HasValue)
            {
                return output;
            }
            // dang fix cung quyen, can lam mem
            if (ErpSession.GroupId.Value == 3)
            {
                output.User.CanWaitingFromDraft = true;
                output.User.CanWaitingFromReturn = true;
                output.User.CanAppoveFromReturn = false;
                output.User.CanAppoveFromWaiting = false;
                output.User.CanDisableFromApprove = false;
                output.User.CanHiddenFromApprove = false;
                output.User.CanReturnFromApprove = false;
                output.User.CanReturnFromWaiting = false;
                output.User.CanCreate = true;
                output.User.CanFilterStatus = true;
            }
            if (ErpSession.GroupId.Value == 4)
            {
                output.User.CanWaitingFromDraft = false;
                output.User.CanWaitingFromReturn = false;
                output.User.CanAppoveFromReturn = true;
                output.User.CanAppoveFromWaiting = true;
                output.User.CanDisableFromApprove = true;
                output.User.CanHiddenFromApprove = true;
                output.User.CanReturnFromApprove = true;
                output.User.CanReturnFromWaiting = true;
                output.User.CanFilterStatus = true;
                output.User.CanCreate = false;
            }
            if (ErpSession.GroupId.Value == 1 || ErpSession.GroupId.Value == 2)
            {
                output.User.CanWaitingFromDraft = false;
                output.User.CanWaitingFromReturn = false;
                output.User.CanAppoveFromReturn = false;
                output.User.CanAppoveFromWaiting = false;
                output.User.CanDisableFromApprove = false;
                output.User.CanHiddenFromApprove = false;
                output.User.CanReturnFromApprove = false;
                output.User.CanReturnFromWaiting = false;
                output.User.CanCreate = false;
            }
            if (output.User.Role == StaticRoleNames.Host.Admin)
            {
                output.User.CanWaitingFromDraft = true;
                output.User.CanWaitingFromReturn = true;
                output.User.CanAppoveFromReturn = true;
                output.User.CanAppoveFromWaiting = true;
                output.User.CanDisableFromApprove = true;
                output.User.CanHiddenFromApprove = true;
                output.User.CanReturnFromApprove = true;
                output.User.CanReturnFromWaiting = true;
                output.User.CanCreate = true;
            }
            return output;
        }
    }
}
