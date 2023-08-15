using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Erpinfo.EntityFrameworkCore;
using Erpinfo.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Erpinfo.Web.Tests
{
    [DependsOn(
        typeof(ErpinfoWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class ErpinfoWebTestModule : AbpModule
    {
        public ErpinfoWebTestModule(ErpinfoEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ErpinfoWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(ErpinfoWebMvcModule).Assembly);
        }
    }
}