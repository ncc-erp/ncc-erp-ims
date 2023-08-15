using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Erpinfo.Controllers
{
    public abstract class ErpinfoControllerBase: AbpController
    {
        protected ErpinfoControllerBase()
        {
            LocalizationSourceName = ErpinfoConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
