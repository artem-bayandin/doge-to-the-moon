using DogeData.Context;
using DogeData.Repos;
using DogeWorker.SochainData;
using Domain.Interfaces;
using Domain.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace DogeWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddHostedService<Worker>();

                    services.AddSingleton<ISochainDataReceiver, SochainDataReceiver>();
                    services.AddSingleton<ITransactionPreparator, TransactionPreparator>();

                    services.AddScoped<IDogeDbContext, DogeDbContext>();
                    services.AddScoped<ITransactionRepo, TransactionRepo>();

                    services
                        .AddDbContext<DogeDbContext>(
                            options =>
                                options.UseSqlServer(
                                    hostContext.Configuration.GetConnectionString("DogeDbContext"),
                                    // enable auto migrations
                                    optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(DogeDbContext).GetTypeInfo().Assembly.GetName().Name)
                                )
                    );
                    //DogeModule.RegisterDb(services, hostContext.Configuration.GetConnectionString("DogeDbContext"));
                });
    }
}
