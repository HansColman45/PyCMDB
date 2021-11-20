using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configuration
{
    class MobileConfiguration : IEntityTypeConfiguration<Mobile>
    {
        public void Configure(EntityTypeBuilder<Mobile> builder)
        {
            builder.ToTable(nameof(Mobile));

            builder.HasKey(e => e.IMEI)
                .HasName("PK_Mobile");

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Mobiles)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Mobile_Identity");

            builder.HasOne(e => e.MobileType)
                .WithMany(d => d.Mobiles)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Mobile_Type");

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Mobiles)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Mobile_Category");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
