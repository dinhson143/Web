using Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.Data.Configurations
{
    public class SliderConfiguration : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.ToTable("Sliders");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();

            builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Url).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Image).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Status).HasDefaultValue(Status.Active);
        }
    }
}