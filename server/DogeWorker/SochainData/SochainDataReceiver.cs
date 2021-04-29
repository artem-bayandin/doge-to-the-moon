using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DogeWorker.SochainData
{
    public class SochainDataReceiverConfig
    {
        public string ReceivedTransactionsEndpoint { get; set; }
        public int MaxReceivedTransactionsPerRequest { get; set; }
        public string GetTransactionEndpoint { get; set; }
    }

    public interface ISochainDataReceiver
    {
        int MaxReceivedTransactionsPerRequest { get; }
        Task<SochainAddressReceivedTransactionsDto> GetAddressTransactions(HttpClient client, string address, string afterTxId = "");
        Task<SochainTransactionDto> GetTransactionData(HttpClient client, string txid);
    }

    public class SochainDataReceiver : ISochainDataReceiver
    {
        private readonly SochainDataReceiverConfig _config;

        public int MaxReceivedTransactionsPerRequest => _config.MaxReceivedTransactionsPerRequest;

        public SochainDataReceiver(IConfiguration configuration)
        {
            _config = configuration.GetSection("SochainDataReceiver").Get<SochainDataReceiverConfig>();
        }

        public async Task<SochainAddressReceivedTransactionsDto> GetAddressTransactions(HttpClient client, string address, string afterTxId = "")
        {
            try
            {
                var response = await client.GetAsync(string.Format(_config.ReceivedTransactionsEndpoint, address, afterTxId));
                string str = await response.Content.ReadAsStringAsync();
                var baseData = JsonConvert.DeserializeObject<SochainDataResponseBase>(str);
                if (baseData.Status == SochainDataResponseBase.Success)
                {
                    var addressData = JsonConvert.DeserializeObject<SochainDataResponse<SochainAddressReceivedTransactionsDto>>(str);
                    return addressData.Data;
                }
                else if (baseData.Status == SochainDataResponseBase.Fail)
                {
                    // TODO: log it to somewhere to analyze later
                    var afterTxString = !string.IsNullOrEmpty(afterTxId) ? $" after txid '{afterTxId}'" : "";
                    Console.WriteLine($"Failed to fetch '{address}' address data{afterTxString}: {str}");
                }
            }
            catch (Exception ex)
            {
                // TODO: log it to somewhere to analyze later
                Console.WriteLine(ex.Message);
            }

            return new SochainAddressReceivedTransactionsDto();
        }

        public async Task<SochainTransactionDto> GetTransactionData(HttpClient client, string txid)
        {
            try
            {
                var response = await client.GetAsync(string.Format(_config.GetTransactionEndpoint, txid));
                string str = await response.Content.ReadAsStringAsync();
                var baseData = JsonConvert.DeserializeObject<SochainDataResponseBase>(str);
                if (baseData.Status == SochainDataResponseBase.Success)
                {
                    var txData = JsonConvert.DeserializeObject<SochainDataResponse<SochainTransactionDto>>(str);
                    return txData.Data;
                }
                else if (baseData.Status == SochainDataResponseBase.Fail)
                {
                    // TODO: log it to somewhere to analyze later
                    Console.WriteLine($"Failed to fetch tx '{txid}': {str}");
                }
            }
            catch (Exception ex)
            {
                // TODO: log it to somewhere to analyze later
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
