using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Erpinfo.Configuration;
using Erpinfo.BackGroundWorker;
using Abp.Threading.BackgroundWorkers;

namespace Erpinfo.Web.Host.Startup
{
    [DependsOn(
       typeof(ErpinfoWebCoreModule))]
    public class ErpinfoWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ErpinfoWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ErpinfoWebHostModule).GetAssembly());
        }
        public override void PostInitialize()
        {
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<Sedulche>());
        }
    }
}
