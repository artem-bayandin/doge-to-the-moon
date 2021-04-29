using DogeWorker.SochainData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DogeData.Repos.InDto;
using Microsoft.Extensions.DependencyInjection;
using DogeData.Repos;

namespace DogeWorker
{
    public class WorkerConfig
    {
        public int PauseMs { get; set; }
        public string RecipientDogeAddress { get; set; }
        public int ConfirmedHaving { get; set; }
    }

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ISochainDataReceiver _dataReceiver;
        private readonly ITransactionPreparator _transactionPreparator;

        private readonly WorkerConfig _config;
        private HttpClient client;
        private string _lastProcessedTransactionId = "";

        public Worker(ILogger<Worker> logger
            , IHttpClientFactory httpClientFactory
            , IServiceScopeFactory scopeFactory
            , ISochainDataReceiver dataReceiver
            , ITransactionPreparator transactionPreparator
            , IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _scopeFactory = scopeFactory;
            _dataReceiver = dataReceiver;
            _transactionPreparator = transactionPreparator;
            _config = configuration.GetSection("Worker").Get<WorkerConfig>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var addressTransactionsCount = 0;
                var maxTransactionsPerRequestProceeded = false;

                do
                {
                    // a. fetch address transactions
                    // TODO: regarding the last fetched one txid
                    var addressTransactions = await _dataReceiver.GetAddressTransactions(client, _config.RecipientDogeAddress, await LastTransactionId(_config.RecipientDogeAddress));
                    addressTransactionsCount = addressTransactions.Txs.Count;

                    // b. process current transactions in a batch
                    if (addressTransactionsCount > 0)
                    {
                        // b.1. fetch all transactions data
                        var transactionDictionary = addressTransactions.Txs.ToDictionary(x => x.TxId, x => x);
                        var tasks = addressTransactions.Txs.Select(tx => _dataReceiver.GetTransactionData(client, tx.TxId)).ToList();
                        Task.WaitAll(tasks.ToArray());
                        var fetchedTransactions = tasks
                            .Select(task =>
                            {
                                var item = task.Result;
                                var tran = new TransactionRecord(
                                    item.TxId,
                                    addressTransactions.Address,
                                    transactionDictionary[item.TxId].Value,
                                    transactionDictionary[item.TxId].Confirmations,
                                    transactionDictionary[item.TxId].Time,
                                    item.Inputs.Select(input => new TransactionInputRecord(input.Address, input.Value)).ToList(),
                                    item.Outputs.Select(output => new TransactionOutputRecord(output.Address, output.Value)).ToList()
                                );

                                return tran;
                            })
                            .OrderBy(x => x.Time)
                            .ToList();

                        // b.2. prepare transactions - check if sender/recipient addresses are valid
                        var preparedTransactions = await _transactionPreparator.PrepareTransactions(fetchedTransactions, _config.ConfirmedHaving);

                        // b.3. save to db and udate users' balances
                        // return lastTransactionId (to understand that all transactions were processed: addressTransactions.indexOf(lastTxId) == addressTransactions.Count)
                        var transactionsSavedResult = await SaveTransactions(preparedTransactions);
                        if (!transactionsSavedResult.Success && transactionsSavedResult.ConcurrentException)
                        {
                            // try once again
                            await Task.Delay(200);
                            transactionsSavedResult = await SaveTransactions(preparedTransactions);
                        }
                        if (transactionsSavedResult.Success && !String.IsNullOrEmpty(transactionsSavedResult.LastTxId))
                        {
                            _lastProcessedTransactionId = transactionsSavedResult.LastTxId;
                        }

                        // b.4. check if all transactions were proceeded
                        maxTransactionsPerRequestProceeded = !String.IsNullOrEmpty(_lastProcessedTransactionId)
                            && addressTransactions
                                    .Txs
                                    .IndexOf(addressTransactions.Txs.FirstOrDefault(tx => tx.TxId == _lastProcessedTransactionId)) == _dataReceiver.MaxReceivedTransactionsPerRequest - 1;
                    }
                }
                while (addressTransactionsCount == _dataReceiver.MaxReceivedTransactionsPerRequest && maxTransactionsPerRequestProceeded); // if 100 fetched - some more may exist

                await Task.Delay(_config.PauseMs, stoppingToken);
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            client = _httpClientFactory.CreateClient();
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            await base.StopAsync(cancellationToken);
        }

        private async Task<string> LastTransactionId(string address)
        {
            if (String.IsNullOrEmpty(_lastProcessedTransactionId))
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<ITransactionRepo>();
                    _lastProcessedTransactionId = await repo.GetLastTransactionId(address);
                }
            }
            return _lastProcessedTransactionId;
        }

        private async Task<(bool Success, bool ConcurrentException, string LastTxId)> SaveTransactions(List<PreparedTransactionRecord> preparedTransactions)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ITransactionRepo>();
            return await repo.InsertFoundTransactions(preparedTransactions);
        }
    }

    public interface ITransactionPreparator
    {
        Task<List<PreparedTransactionRecord>> PrepareTransactions(List<TransactionRecord> fetchedTransactions, int confirmedHaving);
    }

    public class TransactionPreparator : ITransactionPreparator
    {
        public async Task<List<PreparedTransactionRecord>> PrepareTransactions(List<TransactionRecord> fetchedTransactions, int confirmedHaving)
        {
            var preparedTransactions = new List<PreparedTransactionRecord>();

            for (var i = 0; i < fetchedTransactions.Count; i++)
            {
                var current = fetchedTransactions[i];

                // check confirmed
                if (current.Confirmations < confirmedHaving)
                {
                    // treat it as not confirmed and break current cycle
                    break;
                }

                // check inputs
                var inputs = current.Inputs.GroupBy(x => x.Address).ToList();
                var transferredValue = current.Outputs.Where(x => x.Address == current.TargetAddress).Sum(x => x.Value);
                if (inputs.Count > 1)
                {
                    var invalidSenderTransaction = new PreparedTransactionRecord(
                        current.TxId,
                        PreparedTransactionRecord.MultipleSendersText,
                        current.TargetAddress,
                        transferredValue,
                        current.Confirmations,
                        current.Time,
                        current.Inputs, // multi
                        current.Outputs,
                        System.Text.Json.JsonSerializer.Serialize(current),
                        $"Transaction contains too many senders, having {transferredValue} transferred to '{current.TargetAddress}'"
                    );
                    preparedTransactions.Add(invalidSenderTransaction);

                    // transaction is prepared for saving
                    continue;
                }

                // here we have that all inputs are one single sender address

                var senderAddress = inputs.FirstOrDefault().Key;
                var processedTransaction = new PreparedTransactionRecord(
                    current.TxId,
                    senderAddress,
                    current.TargetAddress,
                    transferredValue,
                    current.Confirmations,
                    current.Time,
                    current.Inputs, // only one input here
                    current.Outputs.Where(x => x.Address == current.TargetAddress).ToList(),
                    System.Text.Json.JsonSerializer.Serialize(current),
                    $"Transferred {transferredValue} from '{senderAddress}' to '{current.TargetAddress}'"
                );
                preparedTransactions.Add(processedTransaction);

                // transaction is prepared for saving
            }

            return preparedTransactions;
        }
    }
}
