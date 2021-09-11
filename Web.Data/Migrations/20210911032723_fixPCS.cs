using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class fixPCS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNXchitiets_Products_ProductId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNXchitiets_ProductId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "PhieuNXchitiets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "PhieuNXchitiets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "PCSs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 9, 11, 10, 27, 22, 208, DateTimeKind.Local).AddTicks(6518));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "e0cf3212-8356-4d4d-9c24-3b5871ea584d");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2054604e-4c70-42da-88bb-615d1e92aa6f", "AQAAAAEAACcQAAAAEHFXGkDomE3Qwne0WadFSLv9KM2Hm/QY+hACvWotBzOwiVET83s1qjhGWwpWz9dMtg==" });

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNXchitiets_ProductId_ColorId_SizeId",
                table: "PhieuNXchitiets",
                columns: new[] { "ProductId", "ColorId", "SizeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNXchitiets_PCSs_ProductId_ColorId_SizeId",
                table: "PhieuNXchitiets",
                columns: new[] { "ProductId", "ColorId", "SizeId" },
                principalTable: "PCSs",
                principalColumns: new[] { "ProductId", "ColorId", "SizeId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNXchitiets_PCSs_ProductId_ColorId_SizeId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNXchitiets_ProductId_ColorId_SizeId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "PCSs");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 9, 11, 8, 9, 28, 802, DateTimeKind.Local).AddTicks(5329));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "f5e1f57f-f9c3-4386-b150-78e1a38203df");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "221e7f8a-79e5-4a5d-b434-f650baf8cf3d", "AQAAAAEAACcQAAAAEAxpHCZcZb9++7Ztyed9oXTZVFYo9YYBz2lWiCYNwAbGGmV/NKq8/lMWUqTTLy1bHA==" });

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNXchitiets_ProductId",
                table: "PhieuNXchitiets",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNXchitiets_Products_ProductId",
                table: "PhieuNXchitiets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
