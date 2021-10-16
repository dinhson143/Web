using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class updateLoaiPhieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "LoaiPhieus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 6, 8, 29, 33, 659, DateTimeKind.Local).AddTicks(8656));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "a6e237ea-eca1-42e6-8209-0963f399b607");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 6, 8, 29, 33, 686, DateTimeKind.Local).AddTicks(4447));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 6, 8, 29, 33, 686, DateTimeKind.Local).AddTicks(6700));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 6, 8, 29, 33, 686, DateTimeKind.Local).AddTicks(6715));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "30219643-6c32-40a6-bade-797617f38d02", "AQAAAAEAACcQAAAAEJCskCpxTyZjy96+BAFZLyItvTv+zdh3iF0GuFT1gRIkz18S4WTxeGSodwX4ZSW1Bg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "LoaiPhieus");

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
    }
}
