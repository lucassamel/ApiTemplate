using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTemplate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Products",
                newName: "Count");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "Products",
                newName: "Quantity");
        }
    }
}
