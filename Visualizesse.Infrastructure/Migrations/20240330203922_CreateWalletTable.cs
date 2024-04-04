using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Visualizesse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateWalletTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WalletId",
                table: "Transaction",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallet_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_WalletId",
                table: "Transaction",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallet_WalletId",
                table: "Transaction",
                column: "WalletId",
                principalTable: "Wallet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallet_WalletId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_WalletId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Transaction");
        }
    }
}
