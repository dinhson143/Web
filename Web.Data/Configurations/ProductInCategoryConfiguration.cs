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
    public class ProductInCategoryConfiguration : IEntityTypeConfiguration<ProductInCategory>
    {
        public void Configure(EntityTypeBuilder<ProductInCategory> builder)
        {
            builder.HasKey(x => new { x.CategoryId, x.ProductId });
            builder.ToTable("ProductInCategories");

            // 1 sản phẩm có thể thuộc nhiều loại
            builder.HasOne(x => x.Product).WithMany(y => y.ProductInCategories).HasForeignKey(y => y.ProductId);

            // 1 loại có nhiều sản phẩm
            builder.HasOne(x => x.Category).WithMany(y => y.ProductInCategories).HasForeignKey(y => y.CategoryId);
        }
    }
}