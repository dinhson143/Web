using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Entities;
using Web.Data.Enums;

namespace Web.Data.SeedData
{
    public static class ModelBuilderEntensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //Language
            modelBuilder.Entity<Language>().HasData(
               new Language() { Id = "vi", Name = "Tiếng Việt", IsDefault = true, Status = Status.Active },
               new Language() { Id = "en", Name = "English", IsDefault = false, Status = Status.Active }
           );

            // Category
            modelBuilder.Entity<Category>().HasData(
                 new Category()
                 {
                     Id = 1,
                     IsShowonHome = true,
                     ParentId = null,
                     Status = Status.Active,
                 },
                 new Category()
                 {
                     Id = 2,
                     IsShowonHome = true,
                     ParentId = null,
                     Status = Status.Active,
                 },
                 // cây danh mục cate
                 new Category()
                 {
                     Id = 3,
                     IsShowonHome = true,
                     ParentId = 1,
                     Status = Status.Active,
                 }
            );

            modelBuilder.Entity<CategoryTranslation>().HasData(
                // a
                new CategoryTranslation() { Id = 1, CategoryId = 1, Name = "Gấu Teddy", LanguageId = "vi", SeoAlias = "gau-teddy", SeoDescription = "Gấu bông Teddy", SeoTitle = "Gấu bông Teddy" },
                new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Teddy bear", LanguageId = "en", SeoAlias = "teddy-bear", SeoDescription = "Teddy bear", SeoTitle = "Teddy bear" },
                new CategoryTranslation() { Id = 3, CategoryId = 2, Name = "Thú bông", LanguageId = "vi", SeoAlias = "thu-bong", SeoDescription = "Thú bông", SeoTitle = "Thú bông" },
                new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Stuffed Animal", LanguageId = "en", SeoAlias = "stuffed-animal", SeoDescription = "Stuffed Animal", SeoTitle = "Stuffed Animal" },

                // con cate a-a
                new CategoryTranslation() { Id = 5, CategoryId = 3, Name = "Gấu bông Teddy to", LanguageId = "vi", SeoAlias = "gau-teddy-to", SeoDescription = "Gấu bông Teddy to", SeoTitle = "Gấu bông Teddy to" },
                new CategoryTranslation() { Id = 6, CategoryId = 3, Name = "Big Teddy Bear", LanguageId = "en", SeoAlias = "big-teddy-bear", SeoDescription = "Big Teddy Bear", SeoTitle = "Big Teddy Bear" }
            );

            // Product
            modelBuilder.Entity<Product>().HasData(
                 new Product()
                 {
                     Id = 1,
                     DateCreated = DateTime.Now,
                     ViewCount = 0,
                     Status = Status.Active
                 }
            );

            // product in category
            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { ProductId = 1, CategoryId = 3 }
            );

            modelBuilder.Entity<ProductTranslation>().HasData(
                new ProductTranslation()
                {
                    Id = 1,
                    ProductId = 1,
                    Name = "Gấu Bông Teddy Nhung Áo Đen Đại",
                    LanguageId = "vi",
                    SeoAlias = "gau-bong-teddy-nhung-ao-den-dai",
                    SeoDescription = "Gấu Bông Teddy Nhung Áo Đen Đại",
                    SeoTitle = "Gấu Bông Teddy Nhung Áo Đen Đại",
                    Details = "Gấu Bông Teddy Nhung Áo Đen Đại",
                    Description = "Gấu Bông Teddy Nhung Áo Đen Đại"
                },

                new ProductTranslation()
                {
                    Id = 2,
                    ProductId = 1,
                    Name = "Big Black Velvet Velvet Teddy Bear",
                    LanguageId = "en",
                    SeoAlias = "big-black-velvet-teddy-bear",
                    SeoDescription = "Big Black Velvet Velvet Teddy Bear",
                    SeoTitle = "Big Black Velvet Velvet Teddy Bear",
                    Details = "Big Black Velvet Velvet Teddy Bear",
                    Description = "Big Black Velvet Velvet Teddy Bear"
                }
            );

            // size
            modelBuilder.Entity<Size>().HasData(
                new Size()
                {
                    Id = 1,
                    Name = "50cm",
                    Status = Status.Active
                },
                new Size()
                {
                    Id = 2,
                    Name = "60cm",
                    Status = Status.Active
                },
                new Size()
                {
                    Id = 3,
                    Name = "80cm",
                    Status = Status.Active
                },
                new Size()
                {
                    Id = 4,
                    Name = "1m1",
                    Status = Status.Active
                },
                new Size()
                {
                    Id = 5,
                    Name = "1m4",
                    Status = Status.Active
                }
            );
            // color
            //modelBuilder.Entity<Color>().HasData(
            //    new Color()
            //    {
            //        Id = 1,
            //        Name = "Violet",
            //        Mamau = "#EE82EE"
            //    },
            //    new Color()
            //    {
            //        Id = 2,
            //        Name = "Red",
            //        Mamau = "#FF0000"
            //    },
            //    new Color()
            //    {
            //        Id = 3,
            //        Name = "80cm",
            //        Mamau = "#50c7c7"
            //    },
            //    new Color()
            //    {
            //        Id = 4,
            //        Name = "Orange",
            //        Mamau = "#FFA500"
            //    },
            //    new Color()
            //    {
            //        Id = 5,
            //        Name = "Chocolate",
            //        Mamau = "#D2691E"
            //    }
            //);
            // PCS
            modelBuilder.Entity<Product_Size>().HasData(
               new Product_Size() { ProductId = 1, SizeId = 1, Stock = 0 }
           );
            // User

            // any guid
            var ADMIN_ID = new Guid("B38060F2-8B1C-47AE-80AA-2CF1B518B812");
            // any guid, but nothing is against to use the same one
            var ROLE_ID = new Guid("0D5B7850-46C1-4C80-99C4-D94FC38A3EA7");
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Adminstrator Role ",
                Status = Status.Active
            });

            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = ADMIN_ID,
                UserName = "dinhson",
                NormalizedUserName = "dinhson",
                Email = "dinhson14399@gmail.com",
                NormalizedEmail = "dinhson14399@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Haquyet0506@"),
                SecurityStamp = string.Empty,
                FirstName = "Dinh",
                LastName = "Son",
                Dob = new DateTime(1999, 03, 14),
                Address = "Hcm city",
                Status = Status.Active
            }); ;

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            //Slider
            modelBuilder.Entity<Slider>().HasData(
               new Slider()
               {
                   Id = 1,
                   Name = "slider 1",
                   Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.",
                   Url = "#",
                   DateCreated = DateTime.Now,
                   Image = "https://gaubongonline.vn/wp-content/uploads/2021/06/gaubongonline1.jpg",
                   Status = Status.Active
               },
               new Slider()
               {
                   Id = 2,
                   Name = "slider 1",
                   Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.",
                   Url = "#",
                   DateCreated = DateTime.Now,
                   Image = "https://gaubongonline.vn/wp-content/uploads/2021/06/gaubongonline.jpg",
                   Status = Status.Active
               },
               new Slider()
               {
                   Id = 3,
                   Name = "slider 1",
                   Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.",
                   Url = "#",
                   DateCreated = DateTime.Now,
                   Image = "https://gaubongonline.vn/wp-content/uploads/2021/05/web-gaubongonline.vn_.jpg",
                   Status = Status.Active
               }
           );
        }
    }
}