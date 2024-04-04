using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Visualizesse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionColumnAtWalletTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Wallet",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Wallet");
        }
    }
}
