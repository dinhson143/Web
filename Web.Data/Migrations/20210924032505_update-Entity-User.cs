using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class updateEntityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "Status" },
                values: new object[] { new DateTime(2021, 9, 24, 10, 25, 3, 993, DateTimeKind.Local).AddTicks(5560), 1 });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "c998445a-17c3-4087-a8f5-619e3466ae87");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "SortOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "SortOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 4,
                column: "SortOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "Address", "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "Hcm city", "011685f8-55c7-4876-ab3c-7b58a061c618", "AQAAAAEAACcQAAAAEDVzaAcmFp0tjSo2PLUeIDV0/hT7kbxiIfmKkXlcv0v4b/onyAu1u8X9fipWKIBnXQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "Status" },
                values: new object[] { new DateTime(2021, 9, 19, 11, 14, 5, 815, DateTimeKind.Local).AddTicks(5935), 0 });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "c16d734a-813b-4101-a300-14173387ab03");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "SortOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "SortOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 4,
                column: "SortOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ffea7bd5-c4ed-4a20-972e-5a95991eda94", "AQAAAAEAACcQAAAAEECT8VWehKNE57MRenYs0PUpgkgONWYnegbOjsusXPvWo5BxQejt3zW+drGEnGgs6g==" });
        }
    }
}
