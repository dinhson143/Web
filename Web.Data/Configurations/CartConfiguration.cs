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
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        // 1 user có nhiều cart
        // 1 product có nhiều cart
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            // 1 san pham có thể trong nhiều cart
            builder.HasOne(x => x.Product).WithMany(x => x.Carts).HasForeignKey(x => x.ProductId);

            // 1 user cp1 nhiều cart
            builder.HasOne(x => x.User).WithMany(x => x.Carts).HasForeignKey(x => x.UserId);
        }
    }
}