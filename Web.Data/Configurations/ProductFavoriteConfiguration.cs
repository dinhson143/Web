using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Entities;

namespace Web.Data.Configurations
{
    public class ProductFavoriteConfiguration : IEntityTypeConfiguration<ProductFavorite>
    {
        public void Configure(EntityTypeBuilder<ProductFavorite> builder)
        {
            builder.ToTable("ProductFavorites");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            //
            builder.HasOne(x => x.Product).WithMany(x => x.ProductFavorites).HasForeignKey(x => x.ProductId);

            //
            builder.HasOne(x => x.User).WithMany(x => x.ProductFavorites).HasForeignKey(x => x.UserId);
        }
    }
}