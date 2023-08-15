using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Erpinfo.Configuration;
using Erpinfo.Web;

namespace Erpinfo.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ErpinfoDbContextFactory : IDesignTimeDbContextFactory<ErpinfoDbContext>
    {
        public ErpinfoDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ErpinfoDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            ErpinfoDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ErpinfoConsts.ConnectionStringName));

            return new ErpinfoDbContext(builder.Options);
        }
    }
}
