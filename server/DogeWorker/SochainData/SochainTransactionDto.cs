using System.Collections.Generic;

namespace DogeWorker.SochainData
{
    public class SochainTransactionDto
    {
        public string TxId { get; set; }
        public string BlockHash { get; set; }
        public long Confirmations { get; set; }
        public long Time { get; set; }
        public List<SochainTransactionInputDto> Inputs { get; set; }
        public List<SochainTransactionOutputDto> Outputs { get; set; }
    }

    public class SochainTransactionInputDto
    {
        public long InputNo { get; set; }
        public double Value { get; set; }
        public string Address { get; set; }
        // public string Type { get; set; }
        // public string Script { get; set; }

        /*
        input_no: 0,
value: "3.87700000",
address: "A6KAnTjmUdAEnRsg2nTtjEQSKxbEPxdoFq",
type: "scripthash",
script: "0 304402202ce8d94e9d8877c2d6946accf1832baa0228226c19992258c0e9bf03fce696e302206d596ab3b49c3fd6adcfe9818e51a0330cf3249aa87454b84aee24bd39242dbf01 3045022100c597ae98b068312b453937b9662f4725d66a6f0ec468e9c1d5e9a09ccc136cc30220628b6dafa21bdba4df6f1c13375b6059a65f183c29eb11bdf0b2a23034baea2601 5221032a6f885406f6619abe1cc7420481a75a529f25cca937a40e242476ed32338bad21034976c3c272da865b7891298c06b83aef76b70a88b19cb2f5e7aee4cfbbc3535a52ae",
witness: null,
from_output: {
txid: "f47e82a04531d9aee679b10a2bd1b9091c0b424886aa3c1e5d97fc3230257fbe",
output_no: 1
}
        */
    }

    public class SochainTransactionOutputDto
    {
        public long OutputNo { get; set; }
        public double Value { get; set; }
        public string Address { get; set; }
        // public string Type { get; set; }
        // public string Script { get; set; }

        /*
        output_no: 0,
value: "6.43370000",
address: "D5dS4as5e68WE7j9ZWHdNQGiwV1iNAaXTo",
type: "pubkeyhash",
script: "OP_DUP OP_HASH160 055bf19cbe2f84a51572d35937900e26c3bbe4e5 OP_EQUALVERIFY OP_CHECKSIG"
        */
    }
}
