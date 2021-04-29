using DogeData.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace DogeWorker.DogeDb
{
    public interface IDogeDbContext
    {
        DbSet<TransactionEntity> Transactions { get; set; }
        DbSet<UserEntity> Users { get; set; }

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        //Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        //Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class DogeDbContext : DbContext, IDogeDbContext
    {
        public virtual DbSet<TransactionEntity> Transactions { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }

        public DogeDbContext(DbContextOptions<DogeDbContext> options) : base(options)
        {
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => this.Database.BeginTransactionAsync(cancellationToken);
        //public Task CommitTransactionAsync(CancellationToken cancellationToken = default) => this.Database.CommitTransactionAsync(cancellationToken);
        //public Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => this.Database.RollbackTransactionAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DogeDbContext).Assembly);

            modelBuilder.TypeDateTimeToDatetime2();
            //modelBuilder.TypeStringToNvarchar255();
            modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.SetOnDeleteBehaviorToRestrict();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //// get the configuration from the app settings
            //var config = new ConfigurationBuilder()
            //    // Microsoft.Extensions.Configuration.FileExtensions
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    // Microsoft.Extensions.Configuration.Json
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            //optionsBuilder.UseSqlServer(config.GetConnectionString("EntitiesDatabase"));
        }
    }
}
