using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configuration
{
    public class ScreenConfiguraton : IEntityTypeConfiguration<Screen>
    {
        public void Configure(EntityTypeBuilder<Screen> builder)
        {
            builder.ToTable(nameof(Screen));

            builder.HasKey(e => e.AssetTag)
                .HasName("PK_Screen_Asset");

            builder.Property(e => e.SerialNumber)
                .HasColumnType("varchar(255)");

            builder.HasOne(e => e.Type)
                .WithMany(d => d.Screens)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Screen_Type");

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Screens)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Screen_Identity");

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Screens)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Screen_Category");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
