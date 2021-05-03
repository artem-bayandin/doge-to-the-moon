using DogeData.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DogeData.Context
{
    public class DogeDbContext : IdentityDbContext<UserEntity, UserEntityRole, Guid>, IDogeDbContext
    {
        public virtual DbSet<TransactionEntity> Transactions { get; set; }

        public DogeDbContext(DbContextOptions<DogeDbContext> options) : base(options)
        {
        }

        // public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => this.Database.BeginTransactionAsync(cancellationToken);
        // public Task CommitTransactionAsync(CancellationToken cancellationToken = default) => this.Database.CommitTransactionAsync(cancellationToken);
        // public Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => this.Database.RollbackTransactionAsync(cancellationToken);

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
