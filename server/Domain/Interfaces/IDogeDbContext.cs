using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDogeDbContext : IDisposable
    {
        DbSet<TransactionEntity> Transactions { get; set; }
        DbSet<UserEntity> Users { get; set; }

        // Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        // Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        // Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
