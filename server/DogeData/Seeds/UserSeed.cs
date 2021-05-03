using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DogeData.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder
                .HasData(
                    new UserEntity
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Email = "A6KA@gmail.com",
                        UserName = "A6KA",
                        DogeAddress = "A6KAnTjmUdAEnRsg2nTtjEQSKxbEPxdoFq",
                        Balance = 0
                    },
                    new UserEntity
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Email = "D5dS@gmail.com",
                        UserName = "D5dS",
                        DogeAddress = "D5dS4as5e68WE7j9ZWHdNQGiwV1iNAaXTo",
                        Balance = 0
                    },
                    new UserEntity
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                        Email = "D7Rw@gmail.com",
                        UserName = "D7Rw",
                        DogeAddress = "D7RwRNSFjzX71aWcGQzhspK6rX695R9wmp",
                        Balance = 0
                    },
                    new UserEntity
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                        Email = "DQqe@gmail.com",
                        UserName = "DQqe",
                        DogeAddress = "DQqeTDPaHMEfbmgS9WcRKh7oLby29Xq3cc",
                        Balance = 0
                    }
                );
        }
    }
}
