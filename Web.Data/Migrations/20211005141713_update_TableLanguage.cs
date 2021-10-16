using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class update_TableLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Languages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "en",
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "vi",
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 5, 21, 17, 12, 346, DateTimeKind.Local).AddTicks(1845));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "8f2cc8fe-47e5-4227-94ac-5ad96afc639c");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 5, 21, 17, 12, 366, DateTimeKind.Local).AddTicks(8165));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 5, 21, 17, 12, 366, DateTimeKind.Local).AddTicks(9554));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 5, 21, 17, 12, 366, DateTimeKind.Local).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c671c0ea-db3d-49a3-9b19-ad425788c76c", "AQAAAAEAACcQAAAAEAlOh+FAfbKAgM+K/YbiiT1Lj0wATsSOr3kEFCMlpGY0MkZubtrfTi80d6KJcUg/Ag==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Languages");

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
    }
}
