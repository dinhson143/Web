using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Web.Data.Configurations;
using Web.Data.Entities;
using Web.Data.SeedData;

namespace Web.Data.EF
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Fluent API
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryTranlationConfiguration());
            //modelBuilder.ApplyConfiguration(new ColorConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new CongtyConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new LoaiPhieuConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new PhieuNhap_XuatchitietConfiguration());
            modelBuilder.ApplyConfiguration(new PhieuNhap_XuatConfiguration());
            modelBuilder.ApplyConfiguration(new Product_SizeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductFavoriteConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductInCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTranlationConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionConfiguration());
            modelBuilder.ApplyConfiguration(new OrderOfShipperConfiguration());
            modelBuilder.ApplyConfiguration(new SizeConfiguration());
            modelBuilder.ApplyConfiguration(new SliderConfiguration());
            //modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new Product_SizeConfiguration());

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim").HasKey(x => x.RoleId);
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken").HasKey(x => x.UserId);
            //base.OnModelCreating(modelBuilder);

            // Data seeding
            modelBuilder.Seed();
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }

        //public DbSet<Color> Colors { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<CongTy> CongTys { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LoaiPhieu> LoaiPhieus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PhieuNhap_Xuat> PhieuNXs { get; set; }
        public DbSet<PhieuNhap_Xuatchitiet> PhieuNXchitiets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Size> PCSs { get; set; }
        public DbSet<ProductFavorite> ProductFavorites { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ProductInCategory> ProductInCategories { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }

        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<OrderOfShipper> OrderOfShippers { get; set; }

        public DbSet<Size> Sizes { get; set; }
        public DbSet<Slider> Sliders { get; set; }

        //public DbSet<Transaction> Transactions { get; set; }
    }
}