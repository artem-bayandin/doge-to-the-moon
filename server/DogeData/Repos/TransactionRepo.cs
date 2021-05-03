using Domain.Entities;
using Domain.Interfaces;
using Domain.Repos;
using Domain.Repos.InDto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogeData.Repos
{

    public class TransactionRepo : ITransactionRepo
    {
        private readonly IDogeDbContext _context;

        public TransactionRepo(IDogeDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetLastTransactionId(string address)
        {
            try
            {
                return (await _context
                    .Transactions
                    .Where(x => x.RecipientAddress == address)
                    .OrderByDescending(x => x.TxTime)
                    .AsNoTracking()
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                    ?.TxId;
            }
            catch (Exception ex)
            {
                // log it
                return String.Empty;
            }
        }

        public async Task<(bool Success, bool ConcurrentException, string LastTxId)> InsertFoundTransactions(List<PreparedTransactionRecord> preparedTransactions)
        {
            try
            {
                return (true, false, await InsertFoundTransactionsAttempt(preparedTransactions).ConfigureAwait(false));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // log it
                return (false, true, String.Empty);
            }
            catch (Exception ex)
            {
                // log it
                return (false, false, String.Empty);
            }
        }

        private async Task<string> InsertFoundTransactionsAttempt(List<PreparedTransactionRecord> preparedTransactions)
        {
            var lastTxId = preparedTransactions.OrderByDescending(x => x.Time).FirstOrDefault().TxId;

            // just store transactions atm
            var transactionEntities = preparedTransactions
                .Select(prep => new TransactionEntity
                {
                    TxId = prep.TxId,
                    SenderAddress = prep.SenderAddress,
                    RecipientAddress = prep.RecipientAddress,
                    TxTime = prep.Time,
                    Confirmations = prep.Confirmations,
                    TransferredAmount = prep.Value,
                    InputsJson = JsonConvert.SerializeObject(prep.Inputs),
                    OutputsJson = JsonConvert.SerializeObject(prep.Outputs),
                    Result = prep.Result,
                    TxJson = prep.TxJson
                })
                .ToList();
            await _context.Transactions.AddRangeAsync(transactionEntities).ConfigureAwait(false);

            // increase balances
            var amountsToAdd = preparedTransactions
                .Where(x => x.SenderAddress != PreparedTransactionRecord.MultipleSendersText)
                .GroupBy(x => x.SenderAddress)
                .ToDictionary(x => x.Key, x => x.ToList().Sum(l => l.Value));
            var userAddresses = amountsToAdd.Select(am => am.Key).ToList();
            var users = await _context
                .Users
                .Where(x => userAddresses.Contains(x.DogeAddress))
                .ToListAsync()
                .ConfigureAwait(false);
            foreach (var user in users)
            {
                user.Balance += amountsToAdd[user.DogeAddress];
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return lastTxId;
        }
    }
}
