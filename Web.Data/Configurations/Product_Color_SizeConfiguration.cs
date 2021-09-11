using Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Configurations
{
    public class Product_Color_SizeConfiguration : IEntityTypeConfiguration<Product_Color_Size>
    {
        public void Configure(EntityTypeBuilder<Product_Color_Size> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.ColorId, x.SizeId });
            builder.ToTable("PCSs");
            builder.Property(x => x.Stock).IsRequired().HasDefaultValue(0);
            // 1 sản phẩm có thể thuộc nhiều màu siaze
            builder.HasOne(x => x.Product).WithMany(y => y.PCS).HasForeignKey(y => y.ProductId);

            // 1 màu dùng cho nhiều san pham
            builder.HasOne(x => x.Color).WithMany(y => y.PCS).HasForeignKey(y => y.ColorId);

            // 1 size dùng cho nhiều san pham
            builder.HasOne(x => x.Size).WithMany(y => y.PCS).HasForeignKey(y => y.SizeId);
        }
    }
}