using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Data.Migrations
{
    public partial class Seeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsShowonHome", "ParentId", "SortOrder", "Status" },
                values: new object[,]
                {
                    { 1, true, null, 1, 1 },
                    { 2, true, null, 2, 1 },
                    { 3, true, 1, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "Mamau", "Name" },
                values: new object[,]
                {
                    { 5, "#D2691E", "Chocolate" },
                    { 1, "#EE82EE", "Violet" },
                    { 2, "#FF0000", "Red" },
                    { 3, "#50c7c7", "80cm" },
                    { 4, "#FFA500", "Orange" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "IsDefault", "Name" },
                values: new object[,]
                {
                    { "en-US", false, "English" },
                    { "vi-VN", true, "Tiếng Việt" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "DateCreated", "OriginalPrice", "Price", "Status" },
                values: new object[] { 1, new DateTime(2021, 9, 11, 8, 9, 28, 802, DateTimeKind.Local).AddTicks(5329), 100000m, 200000m, 0 });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"), "f5e1f57f-f9c3-4386-b150-78e1a38203df", "Adminstrator Role ", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "80cm" },
                    { 1, "50cm" },
                    { 2, "60cm" },
                    { 5, "1m4" },
                    { 4, "1m1" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"), new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"), 0, "221e7f8a-79e5-4a5d-b434-f650baf8cf3d", new DateTime(1999, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "dinhson14399@gmail.com", true, "Dinh", "Son", false, null, "dinhson14399@gmail.com", "admin", "AQAAAAEAACcQAAAAEAxpHCZcZb9++7Ztyed9oXTZVFYo9YYBz2lWiCYNwAbGGmV/NKq8/lMWUqTTLy1bHA==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "CategoryTranslations",
                columns: new[] { "Id", "CategoryId", "LanguageId", "Name", "SeoAlias", "SeoDescription", "SeoTitle" },
                values: new object[,]
                {
                    { 1, 1, "vi-VN", "Gấu Teddy", "gau-teddy", "Gấu bông Teddy", "Gấu bông Teddy" },
                    { 3, 2, "vi-VN", "Thú bông", "thu-bong", "Thú bông", "Thú bông" },
                    { 5, 3, "vi-VN", "Gấu bông Teddy to", "gau-teddy-to", "Gấu bông Teddy to", "Gấu bông Teddy to" },
                    { 2, 1, "en-US", "Teddy bear", "teddy-bear", "Teddy bear", "Teddy bear" },
                    { 4, 2, "en-US", "Stuffed Animal", "stuffed-animal", "Stuffed Animal", "Stuffed Animal" },
                    { 6, 3, "en-US", "Big Teddy Bear", "big-teddy-bear", "Big Teddy Bear", "Big Teddy Bear" }
                });

            migrationBuilder.InsertData(
                table: "PCSs",
                columns: new[] { "ColorId", "ProductId", "SizeId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "ProductInCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[] { 3, 1 });

            migrationBuilder.InsertData(
                table: "ProductTranslations",
                columns: new[] { "Id", "Description", "Details", "LanguageId", "Name", "ProductId", "SeoAlias", "SeoDescription", "SeoTitle" },
                values: new object[,]
                {
                    { 1, "Gấu Bông Teddy Nhung Áo Đen Đại", "Gấu Bông Teddy Nhung Áo Đen Đại", "vi-VN", "Gấu Bông Teddy Nhung Áo Đen Đại", 1, "gau-bong-teddy-nhung-ao-den-dai", "Gấu Bông Teddy Nhung Áo Đen Đại", "Gấu Bông Teddy Nhung Áo Đen Đại" },
                    { 2, "Big Black Velvet Velvet Teddy Bear", "Big Black Velvet Velvet Teddy Bear", "en-US", "Big Black Velvet Velvet Teddy Bear", 1, "big-black-velvet-teddy-bear", "Big Black Velvet Velvet Teddy Bear", "Big Black Velvet Velvet Teddy Bear" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PCSs",
                keyColumns: new[] { "ColorId", "ProductId", "SizeId" },
                keyValues: new object[] { 1, 1, 1 });

            migrationBuilder.DeleteData(
                table: "ProductInCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductTranslations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"), new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "en-US");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "vi-VN");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
