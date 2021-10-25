using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class addstatus_thanhtoan_table_order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThanhToan",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 19, 8, 11, 15, 120, DateTimeKind.Local).AddTicks(9267));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "76c64fff-703e-4b70-ab7a-c3ddc684cd7f");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 19, 8, 11, 15, 207, DateTimeKind.Local).AddTicks(5314));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 19, 8, 11, 15, 208, DateTimeKind.Local).AddTicks(238));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 19, 8, 11, 15, 208, DateTimeKind.Local).AddTicks(260));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6733e202-bbf4-43cb-845c-51eeb37f6253", "AQAAAAEAACcQAAAAEKhsG0YtcDHc9t4IPB8j7AoUmVu/D3SeDDvITo49bbb2INFkQYx5QRttqPjQr9vYkw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThanhToan",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 13, 19, 45, 16, 487, DateTimeKind.Local).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "ff454ebd-2b4e-4fb9-918c-0850adf8377f");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 13, 19, 45, 16, 520, DateTimeKind.Local).AddTicks(498));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 13, 19, 45, 16, 520, DateTimeKind.Local).AddTicks(2419));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 13, 19, 45, 16, 520, DateTimeKind.Local).AddTicks(2430));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5a0e520b-2a11-483f-b8b2-895ab1496ffd", "AQAAAAEAACcQAAAAEEeAli6C+11pIc1dn52uA7RC+D3VcynSgvX7xpltWLvB+/J5HtSYZbB5/ipPq5B5eA==" });
        }
    }
}
