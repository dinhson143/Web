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
    public class PhieuNhap_XuatchitietConfiguration : IEntityTypeConfiguration<PhieuNhap_Xuatchitiet>
    {
        public void Configure(EntityTypeBuilder<PhieuNhap_Xuatchitiet> builder)
        {
            builder.ToTable("PhieuNXchitiets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // 1 PhieuNX có nhiều chi tiết Phiếu NX
            builder.HasOne(x => x.PhieuNX).WithMany(x => x.PhieuNXchitiets).HasForeignKey(x => x.PhieuNXId);
            // 1 Product có nhiều chi tiết Phiếu NX
            builder.HasOne(x => x.Product).WithMany(x => x.PhieuNXchitiets).HasForeignKey(x => x.ProductId);
        }
    }
}