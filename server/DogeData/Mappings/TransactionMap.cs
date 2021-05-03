using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DogeData.Mappings
{
    public class TransactionMap : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.HasKey(x => x.TxId);
            builder.HasIndex(x => x.SenderAddress);
            builder.HasIndex(x => x.RecipientAddress);
        }
    }

    public class UserMap : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasAlternateKey(x => x.Email);
            builder.HasAlternateKey(x => x.DogeAddress);
            builder.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
