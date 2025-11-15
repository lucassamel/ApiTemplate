using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTemplate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeUTC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValue: new DateTime(2025, 11, 15, 12, 19, 22, 429, DateTimeKind.Utc).AddTicks(6522));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValue: new DateTime(2025, 11, 15, 12, 19, 22, 429, DateTimeKind.Utc).AddTicks(5997));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Products",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValue: new DateTime(2025, 11, 15, 12, 19, 22, 428, DateTimeKind.Utc).AddTicks(5679));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValue: new DateTime(2025, 11, 15, 12, 19, 22, 425, DateTimeKind.Utc).AddTicks(9200));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 15, 12, 19, 22, 429, DateTimeKind.Utc).AddTicks(6522),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 15, 12, 19, 22, 429, DateTimeKind.Utc).AddTicks(5997),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Products",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 15, 12, 19, 22, 428, DateTimeKind.Utc).AddTicks(5679),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 15, 12, 19, 22, 425, DateTimeKind.Utc).AddTicks(9200),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
