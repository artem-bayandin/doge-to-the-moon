using System.Collections.Generic;

namespace DogeWorker.Dtos
{
    /*
    public class TransactionDto
    {
        public string TargetAddress { get; }
        public string TxId { get; }
        public string Value { get; }
        public long Confirmations { get; }
        public long Time { get; }
        public List<TransactionInputDto> Inputs { get; }
        public List<TransactionOutputDto> Outputs { get; }

        public TransactionDto(string targetAddress, string txId, string value, long confirmations, long time, List<TransactionInputDto> inputs, List<TransactionOutputDto> outputs)
        {
            TargetAddress = targetAddress;
            TxId = txId;
            Value = value;
            Confirmations = confirmations;
            Time = time;
            Inputs = inputs;
            Outputs = outputs;
        }

        public override string ToString()
        {
            return $"{Value} to '{TargetAddress}', confirmed: {Confirmations}";
        }
    }

    public class TransactionInputDto
    {
        public string Address { get; }
        public double Value { get; }

        public TransactionInputDto(string address, double value)
        {
            Address = address;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Value} from '{Address}'";
        }
    }

    public class TransactionOutputDto
    {
        public string Address { get; }
        public double Value { get; }

        public TransactionOutputDto(string address, double value)
        {
            Address = address;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Value} to '{Address}'";
        }
    }
    */
}
