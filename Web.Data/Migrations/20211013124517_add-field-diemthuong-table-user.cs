using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class addfielddiemthuongtableuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Diem",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diem",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 13, 9, 58, 41, 188, DateTimeKind.Local).AddTicks(6209));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "c0d20f94-c6ff-4c45-a5d7-8a0ddf98373a");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 13, 9, 58, 41, 214, DateTimeKind.Local).AddTicks(8712));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 13, 9, 58, 41, 215, DateTimeKind.Local).AddTicks(1441));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 13, 9, 58, 41, 215, DateTimeKind.Local).AddTicks(1456));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0ffdb18f-8414-4951-aecd-dd99efe114bd", "AQAAAAEAACcQAAAAEAyVwMf3j2U9Eo9+B0Kfn74B0P8tKLNR9d6xfWLy324nLBfOKuZpVysqQhnP+Fpw0w==" });
        }
    }
}
