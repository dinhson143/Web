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
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");
            builder.HasKey(x => new { x.OrderId, x.ProductId });

            // 1 Order có nhiều orderdetail

            builder.HasOne(x => x.Order).WithMany(y => y.OrderDetails).HasForeignKey(y => y.OrderId);

            // 1 product có thể nằm trong nhiều orderdetail

            builder.HasOne(x => x.Product).WithMany(y => y.OrderDetails).HasForeignKey(y => y.ProductId);
        }
    }
}