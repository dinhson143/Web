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
    public class ShipperOrderConfiguration : IEntityTypeConfiguration<ShipperOrder>
    {
        public void Configure(EntityTypeBuilder<ShipperOrder> builder)
        {
            builder.ToTable("ShipperOrders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ShipperId).IsRequired();
            builder.Property(x => x.OrderID).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Date).IsRequired();
        }
    }
}