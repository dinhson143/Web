using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class createtableOrderOfShipper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderOfShippers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOfShippers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 11, 5, 20, 56, 53, 369, DateTimeKind.Local).AddTicks(1242));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "13bcd755-89da-42b8-aaa2-d7022c28d042");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 11, 5, 20, 56, 53, 390, DateTimeKind.Local).AddTicks(6606));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 11, 5, 20, 56, 53, 390, DateTimeKind.Local).AddTicks(8153));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 11, 5, 20, 56, 53, 390, DateTimeKind.Local).AddTicks(8160));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d452bf96-d920-44b0-a3da-c169983826fd", "AQAAAAEAACcQAAAAEBeNNIBm+s89/nfCwk7DrJRtDPCe0lietRm+u73p3ymI1UOgoKZ3k5c3pX3uvZqJyw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderOfShippers");

            migrationBuilder.CreateTable(
                name: "ShipperOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipperOrders", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 11, 3, 21, 11, 1, 882, DateTimeKind.Local).AddTicks(9357));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "07fb42fd-3369-4713-8d7e-60915c9c97f1");

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 11, 3, 21, 11, 1, 902, DateTimeKind.Local).AddTicks(1275));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2021, 11, 3, 21, 11, 1, 902, DateTimeKind.Local).AddTicks(2531));

            migrationBuilder.UpdateData(
                table: "Sliders",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2021, 11, 3, 21, 11, 1, 902, DateTimeKind.Local).AddTicks(2537));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6ea27d8d-3045-4b7d-b205-1f8f8fa9d432", "AQAAAAEAACcQAAAAEBNE45EF1ybzGqMhJWxFGFjvyL5gUAIS10uMXJE/XLwTMOyimReYavZXohRLaBmLTQ==" });
        }
    }
}