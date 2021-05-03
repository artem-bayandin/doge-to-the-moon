using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
    public class TransactionEntity
    {
        public string TxId { get; set; }
        public string SenderAddress { get; set; }
        public string RecipientAddress { get; set; }
        public double TransferredAmount { get; set; }
        public long Confirmations { get; set; }
        public long TxTime { get; set; }
        public string InputsJson { get; set; }
        public string OutputsJson { get; set; }
        public string TxJson { get; set; }
        public string Result { get; set; }
    }

    public class UserEntity : IdentityUser<Guid>
    {
        //public Guid Id { get; set; }
        //public string Email { get; set; }
        //public string Username { get; set; }
        public string DogeAddress { get; set; }
        public double Balance { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class UserEntityRole : IdentityRole<Guid>
    {

    }
}
