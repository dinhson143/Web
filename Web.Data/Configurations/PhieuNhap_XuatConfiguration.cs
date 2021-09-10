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
    public class PhieuNhap_XuatConfiguration : IEntityTypeConfiguration<PhieuNhap_Xuat>
    {
        public void Configure(EntityTypeBuilder<PhieuNhap_Xuat> builder)
        {
            builder.ToTable("PhieuNXs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // 1 công ty có nhiều phiếu NX
            builder.HasOne(x => x.CongTy).WithMany(x => x.PhieuNXs).HasForeignKey(x => x.CongTyId);
            // 1 loại phieu có nhiều phiếu NX
            builder.HasOne(x => x.LoaiPhieu).WithMany(x => x.PhieuNXs).HasForeignKey(x => x.LoaiPhieuId);
        }
    }
}