using DogeData;
using DogeData.Repos;
using DogeWorker.DogeDb;
using DogeWorker.SochainData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

                    DogeModule.RegisterDb(services, hostContext.Configuration.GetConnectionString("DogeDbContext"));
                });
    }
}
