using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class updateTableCongty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CongTys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 5, 19, 28, 36, 614, DateTimeKind.Local).AddTicks(9759));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "8f632df9-48b3-4800-97bb-2e4c0b4c9ce6");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 5, 19, 28, 36, 647, DateTimeKind.Local).AddTicks(2258));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 5, 19, 28, 36, 647, DateTimeKind.Local).AddTicks(3993));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 5, 19, 28, 36, 647, DateTimeKind.Local).AddTicks(4003));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a9e56f95-48cb-4333-942a-aed68efefe4d", "AQAAAAEAACcQAAAAEEE2x6fVC6kzbNWmVEHCcnmQ/OXLV5Z3pSIN4XTi24hs1daP0ZfiRM/KtWgmLrUiEA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CongTys");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 4, 9, 29, 39, 751, DateTimeKind.Local).AddTicks(3821));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "5a63b4fa-b171-428d-b6cc-e633bbcca79a");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 4, 9, 29, 39, 775, DateTimeKind.Local).AddTicks(6976));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 4, 9, 29, 39, 775, DateTimeKind.Local).AddTicks(8492));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 4, 9, 29, 39, 775, DateTimeKind.Local).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ed8a4bee-f9b9-4f2b-b3cc-97b90a133a35", "AQAAAAEAACcQAAAAEHf3u8ioc0ToU+eWrfCwP/VuOC+ioATFkTuHvJmVMwVFI7Td9eLnoLx7z/DScPFyYw==" });
        }
    }
}
