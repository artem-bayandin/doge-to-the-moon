using System.Collections.Generic;

namespace DogeData.Repos.InDto
{
    public record PreparedTransactionRecord
    (
        string TxId
        , string SenderAddress
        , string RecipientAddress
        , double Value
        , long Confirmations
        , long Time
        , List<TransactionInputRecord> Inputs
        , List<TransactionOutputRecord> Outputs
        , string TxJson
        , string Result
    )
    {
        public const string MultipleSendersText = "[multiple senders]";
    }

    public record TransactionRecord
    (
        string TxId
        , string TargetAddress
        , double Value
        , long Confirmations
        , long Time
        , List<TransactionInputRecord> Inputs
        , List<TransactionOutputRecord> Outputs
    )
    {
        public override string ToString()
        {
            return $"{Value} to '{TargetAddress}', confirmed: {Confirmations}";
        }
    }

    public record TransactionInputRecord(string Address, double Value)
    {
        public override string ToString()
        {
            return $"{Value} from '{Address}'";
        }
    }

    public record TransactionOutputRecord(string Address, double Value)
    {
        public override string ToString()
        {
            return $"{Value} to '{Address}'";
        }
    }
}
