using Web.Data.Entities;
using Web.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Status).HasDefaultValue(Status.Active);
            builder.Property(x => x.Content).IsRequired().HasMaxLength(200);

            // 1 product có nhiều comment
            builder.HasOne(x => x.Product).WithMany(x => x.Comments).HasForeignKey(x => x.ProductId);
            // 1 user có nhiều comment
            builder.HasOne(x => x.User).WithMany(x => x.Comments).HasForeignKey(x => x.UserId);
        }
    }
}