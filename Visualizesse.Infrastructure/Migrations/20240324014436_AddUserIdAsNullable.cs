using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Visualizesse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdAsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Subcategory",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Subcategory",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
