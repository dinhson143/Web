using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class update_PCS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCSs_Colors_ColorId",
                table: "PCSs");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNXchitiets_PCSs_ProductId_ColorId_SizeId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNXchitiets_ProductId_ColorId_SizeId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PCSs",
                table: "PCSs");

            migrationBuilder.DropIndex(
                name: "IX_PCSs_ColorId",
                table: "PCSs");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "PCSs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PCSs",
                table: "PCSs",
                columns: new[] { "ProductId", "SizeId" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 21, 43, 12, 200, DateTimeKind.Local).AddTicks(650));

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

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNXchitiets_ProductId_SizeId",
                table: "PhieuNXchitiets",
                columns: new[] { "ProductId", "SizeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNXchitiets_PCSs_ProductId_SizeId",
                table: "PhieuNXchitiets",
                columns: new[] { "ProductId", "SizeId" },
                principalTable: "PCSs",
                principalColumns: new[] { "ProductId", "SizeId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNXchitiets_PCSs_ProductId_SizeId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNXchitiets_ProductId_SizeId",
                table: "PhieuNXchitiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PCSs",
                table: "PCSs");

            migrationBuilder.DeleteData(
                table: "PCSs",
                keyColumns: new[] { "ProductId", "SizeId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "PhieuNXchitiets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "PCSs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PCSs",
                table: "PCSs",
                columns: new[] { "ProductId", "ColorId", "SizeId" });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mamau = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "Mamau", "Name" },
                values: new object[,]
                {
                    { 1, "#EE82EE", "Violet" },
                    { 2, "#FF0000", "Red" },
                    { 3, "#50c7c7", "80cm" },
                    { 4, "#FFA500", "Orange" },
                    { 5, "#D2691E", "Chocolate" }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 20, 47, 27, 193, DateTimeKind.Local).AddTicks(7358));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "8a22e66c-2b0b-4330-bc43-3e3d0ed797a0");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 20, 47, 27, 223, DateTimeKind.Local).AddTicks(1377));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 20, 47, 27, 223, DateTimeKind.Local).AddTicks(2781));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 2, 20, 47, 27, 223, DateTimeKind.Local).AddTicks(2789));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "98a3691e-f33e-4f25-bc18-a88000444c7b", "AQAAAAEAACcQAAAAEIF5Yjf6UtwprxlufBs0m/5aQYiXVUb2DKG9c6Gq8XQpv5Vk9+OAYp5kqAgFclWOrg==" });

            migrationBuilder.InsertData(
                table: "PCSs",
                columns: new[] { "ColorId", "ProductId", "SizeId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNXchitiets_ProductId_ColorId_SizeId",
                table: "PhieuNXchitiets",
                columns: new[] { "ProductId", "ColorId", "SizeId" });

            migrationBuilder.CreateIndex(
                name: "IX_PCSs_ColorId",
                table: "PCSs",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PCSs_Colors_ColorId",
                table: "PCSs",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNXchitiets_PCSs_ProductId_ColorId_SizeId",
                table: "PhieuNXchitiets",
                columns: new[] { "ProductId", "ColorId", "SizeId" },
                principalTable: "PCSs",
                principalColumns: new[] { "ProductId", "ColorId", "SizeId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
