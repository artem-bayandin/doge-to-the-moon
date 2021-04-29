using DogeWorker.DogeDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DogeData
{
    public static class DogeModule
    {
        public static void RegisterDb(IServiceCollection services, string connectionString)
        {
            services
                .AddDbContext<DogeDbContext>(
                    options =>
                        options.UseSqlServer(
                            connectionString,
                            // enable auto migrations
                            optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(DogeDbContext).GetTypeInfo().Assembly.GetName().Name)
                        )
                    );
        }
    }
}
