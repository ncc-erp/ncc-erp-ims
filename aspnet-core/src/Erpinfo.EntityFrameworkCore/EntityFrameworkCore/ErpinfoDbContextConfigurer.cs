using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Erpinfo.EntityFrameworkCore
{
    public static class ErpinfoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ErpinfoDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ErpinfoDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
