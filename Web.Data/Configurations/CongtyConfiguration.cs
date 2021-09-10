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
    public class CongtyConfiguration : IEntityTypeConfiguration<CongTy>
    {
        public void Configure(EntityTypeBuilder<CongTy> builder)
        {
            builder.ToTable("CongTys");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Diachi).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Masothue).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Sdt).IsRequired().IsUnicode(false).HasMaxLength(20);
        }
    }
}