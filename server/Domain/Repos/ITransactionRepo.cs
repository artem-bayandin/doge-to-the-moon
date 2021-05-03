using Domain.Repos.InDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repos
{
    public interface ITransactionRepo
    {
        Task<string> GetLastTransactionId(string address);
        Task<(bool Success, bool ConcurrentException, string LastTxId)> InsertFoundTransactions(List<PreparedTransactionRecord> preparedTransactions);
    }
}
