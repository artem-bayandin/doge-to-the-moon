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
                        NormalizedEmail = "A6KA@gmail.com",
                        EmailConfirmed = true,
                        UserName = "A6KA",
                        NormalizedUserName = "A6KA",
                        DogeAddress = "A6KAnTjmUdAEnRsg2nTtjEQSKxbEPxdoFq",
                        Balance = 0,
                        SecurityStamp = Guid.NewGuid().ToString()
                    },
                    new UserEntity
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Email = "D5dS@gmail.com",
                        NormalizedEmail = "D5dS@gmail.com",
                        EmailConfirmed = true,
                        UserName = "D5dS",
                        NormalizedUserName = "D5dS",
                        DogeAddress = "D5dS4as5e68WE7j9ZWHdNQGiwV1iNAaXTo",
                        Balance = 0,
                        SecurityStamp = Guid.NewGuid().ToString()
                    },
                    new UserEntity
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                        Email = "D7Rw@gmail.com",
                        NormalizedEmail = "D7Rw@gmail.com",
                        EmailConfirmed = true,
                        UserName = "D7Rw",
                        NormalizedUserName = "D7Rw",
                        DogeAddress = "D7RwRNSFjzX71aWcGQzhspK6rX695R9wmp",
                        Balance = 0,
                        SecurityStamp = Guid.NewGuid().ToString()
                    },
                    new UserEntity
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                        Email = "DQqe@gmail.com",
                        NormalizedEmail = "DQqe@gmail.com",
                        EmailConfirmed = true,
                        UserName = "DQqe",
                        NormalizedUserName = "DQqe",
                        DogeAddress = "DQqeTDPaHMEfbmgS9WcRKh7oLby29Xq3cc",
                        Balance = 0,
                        SecurityStamp = Guid.NewGuid().ToString()
                    }
                );
        }
    }
}
