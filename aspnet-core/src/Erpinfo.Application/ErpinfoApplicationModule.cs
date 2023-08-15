using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Erpinfo.Authorization;

namespace Erpinfo
{
    [DependsOn(
        typeof(ErpinfoCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ErpinfoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ErpinfoAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ErpinfoApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
