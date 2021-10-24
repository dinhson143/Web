using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class addtableshipperOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShipperOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipperOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipperOrders_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipperOrders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 22, 21, 37, 26, 260, DateTimeKind.Local).AddTicks(7384));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "4a897407-a71f-404f-8121-5e5e7b5c1605");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 22, 21, 37, 26, 284, DateTimeKind.Local).AddTicks(2293));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 22, 21, 37, 26, 284, DateTimeKind.Local).AddTicks(3999));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 22, 21, 37, 26, 284, DateTimeKind.Local).AddTicks(4008));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8bb4b55e-bc3e-44a3-9b68-bd278146757f", "AQAAAAEAACcQAAAAEBYiN0LA/7L0LYPClFXaxuFrglkQfwG0Ug8Hu0fKhWLGjrPIXGiZzwTOWbYW/8H7fw==" });

            migrationBuilder.CreateIndex(
                name: "IX_ShipperOrders_OrderID",
                table: "ShipperOrders",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ShipperOrders_UserId",
                table: "ShipperOrders",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipperOrders");

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
    }
}
