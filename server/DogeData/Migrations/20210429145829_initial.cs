using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogeData.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionEntity",
                columns: table => new
                {
                    TxId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderAddress = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecipientAddress = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransferredAmount = table.Column<double>(type: "float", nullable: false),
                    Confirmations = table.Column<long>(type: "bigint", nullable: false),
                    TxTime = table.Column<long>(type: "bigint", nullable: false),
                    InputsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutputsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TxJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionEntity", x => x.TxId);
                });

            migrationBuilder.CreateTable(
                name: "UserEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DogeAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntity", x => x.Id);
                    table.UniqueConstraint("AK_UserEntity_DogeAddress", x => x.DogeAddress);
                    table.UniqueConstraint("AK_UserEntity_Email", x => x.Email);
                });

            migrationBuilder.InsertData(
                table: "UserEntity",
                columns: new[] { "Id", "Balance", "DogeAddress", "Email", "Username" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), 0.0, "A6KAnTjmUdAEnRsg2nTtjEQSKxbEPxdoFq", "A6KA@gmail.com", "A6KA" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), 0.0, "D5dS4as5e68WE7j9ZWHdNQGiwV1iNAaXTo", "D5dS@gmail.com", "D5dS" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), 0.0, "D7RwRNSFjzX71aWcGQzhspK6rX695R9wmp", "D7Rw@gmail.com", "D7Rw" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), 0.0, "DQqeTDPaHMEfbmgS9WcRKh7oLby29Xq3cc", "DQqe@gmail.com", "DQqe" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntity_RecipientAddress",
                table: "TransactionEntity",
                column: "RecipientAddress");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntity_SenderAddress",
                table: "TransactionEntity",
                column: "SenderAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionEntity");

            migrationBuilder.DropTable(
                name: "UserEntity");
        }
    }
}
