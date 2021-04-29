using System.Collections.Generic;

namespace DogeWorker.SochainData
{
    public class SochainAddressReceivedTransactionsDto
    {
        public string Network { get; set; }
        public string Address { get; set; }
        public List<SochainAddressReceivedTransactionDto> Txs { get; set; }
    }

    public class SochainAddressReceivedTransactionDto
    {
        public string TxId { get; set; }
        public double Value { get; set; }
        public long Confirmations { get; set; }
        public long Time { get; set; }
    }
}
