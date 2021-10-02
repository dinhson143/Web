using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class updateProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "PCSs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "PCSs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 22, 2, 34, 284, DateTimeKind.Local).AddTicks(8217));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "5ceeb549-89bb-4408-9725-b9078b25dca6");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 22, 2, 34, 304, DateTimeKind.Local).AddTicks(7254));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 22, 2, 34, 304, DateTimeKind.Local).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 22, 2, 34, 304, DateTimeKind.Local).AddTicks(8543));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "176570d2-0426-4ef7-a09c-ce3688faa4d8", "AQAAAAEAACcQAAAAEEGFfFKgNOlAEz0fGc1r00v9GBz/8P9J53xQOKW8Exdx1A7i0jtwRWf5gj4Xhavtlw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "PCSs");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "PCSs");

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "OriginalPrice", "Price" },
                values: new object[] { new DateTime(2021, 10, 2, 21, 43, 12, 200, DateTimeKind.Local).AddTicks(650), 100000m, 200000m });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "c2a1752e-385e-45d5-ac95-9b5705cdf56e");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 21, 43, 12, 220, DateTimeKind.Local).AddTicks(6878));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 21, 43, 12, 220, DateTimeKind.Local).AddTicks(8245));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 21, 43, 12, 220, DateTimeKind.Local).AddTicks(8252));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a09cac0c-dac2-49e3-a63f-43230fe9b41c", "AQAAAAEAACcQAAAAEJDv2DQT7xBa1UwaGbOE8/e9KgQwxXEoCvT3GVR/qH2Ls4Flu683UCiZe5u39Mqgaw==" });
        }
    }
}
