using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class deletetabletransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 9, 19, 10, 41, 15, 894, DateTimeKind.Local).AddTicks(2418));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "ee8ca763-563a-4a33-a200-fbddac6c58e9");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "950618b2-142d-4639-a0a8-533413c9883c", "AQAAAAEAACcQAAAAEDfZDLZublbfG3lmY9zQH/0njzr287Dr6FDHcIh7P/FC1/ormU7wC+nyvYPfijwP2g==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExternalTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 9, 19, 10, 20, 19, 476, DateTimeKind.Local).AddTicks(6126));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"),
                column: "ConcurrencyStamp",
                value: "85a18596-58fc-4ca4-b2fd-3bbc26bb731e");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c3d896ff-96cc-469b-855f-ef1e92daf943", "AQAAAAEAACcQAAAAEFqQTMQ9OkDrv1sij0iHWhnVKajHaa0aK/mUX3FJr+55em0WB6oYbovHYHdmugn+7w==" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");
        }
    }
}
