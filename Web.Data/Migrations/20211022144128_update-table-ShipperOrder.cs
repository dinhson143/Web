using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class updatetableShipperOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipperOrders_Orders_OrderID",
                table: "ShipperOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipperOrders_Users_UserId",
                table: "ShipperOrders");

            migrationBuilder.DropIndex(
                name: "IX_ShipperOrders_OrderID",
                table: "ShipperOrders");

            migrationBuilder.DropIndex(
                name: "IX_ShipperOrders_UserId",
                table: "ShipperOrders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShipperOrders");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 22, 21, 41, 27, 268, DateTimeKind.Local).AddTicks(6307));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "c6aa1fcd-6c9a-40df-8159-afead27c60f2");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 10, 22, 21, 41, 27, 291, DateTimeKind.Local).AddTicks(3183));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 10, 22, 21, 41, 27, 291, DateTimeKind.Local).AddTicks(4525));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 10, 22, 21, 41, 27, 291, DateTimeKind.Local).AddTicks(4534));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "300c4e73-7ac7-404f-b8d8-411ced300e17", "AQAAAAEAACcQAAAAEJWbqoD4AdFT6/nMpXNtMVfN25+AcpIqH6ZOeTqjBvEdveUHmE0mYhZPF6Dj4CyQeQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ShipperOrders",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ShipperOrders_Orders_OrderID",
                table: "ShipperOrders",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShipperOrders_Users_UserId",
                table: "ShipperOrders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
