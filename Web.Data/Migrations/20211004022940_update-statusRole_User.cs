using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class updatestatusRole_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                columns: new[] { "ConcurrencyStamp", "Status" },
                values: new object[] { "5a63b4fa-b171-428d-b6cc-e633bbcca79a", 1 });

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Status" },
                values: new object[] { "ed8a4bee-f9b9-4f2b-b3cc-97b90a133a35", "AQAAAAEAACcQAAAAEHf3u8ioc0ToU+eWrfCwP/VuOC+ioATFkTuHvJmVMwVFI7Td9eLnoLx7z/DScPFyYw==", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 3, 9, 16, 1, 298, DateTimeKind.Local).AddTicks(7860));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "ef0c00cf-c7e4-4c22-9b11-4d5c95a2a60a");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 3, 9, 16, 1, 319, DateTimeKind.Local).AddTicks(6710));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 3, 9, 16, 1, 319, DateTimeKind.Local).AddTicks(8114));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 3, 9, 16, 1, 319, DateTimeKind.Local).AddTicks(8121));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "55091433-b4d7-407b-ad6e-c4a93b8f0d0d", "AQAAAAEAACcQAAAAEDoJJAEj+ojUA3CNsNzRodMT7duvWDONO7PPXgQUg0oNOsotYm35lSNZNiyJT6ZaGQ==" });
        }
    }
}
