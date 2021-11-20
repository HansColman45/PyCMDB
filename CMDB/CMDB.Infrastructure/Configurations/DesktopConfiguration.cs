using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configuration
{
    public class DesktopConfiguraton : IEntityTypeConfiguration<Desktop>
    {
        public void Configure(EntityTypeBuilder<Desktop> builder)
        {
            builder.ToTable(nameof(Desktop));

            builder.HasKey(e => e.AssetTag)
                .HasName("PK_Desktop_Asset");

            builder.Property(e => e.SerialNumber)
                .HasColumnType("varchar(255)");

            builder.HasOne(e => e.Type)
                .WithMany(d => d.Desktops)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Desktop_Type");

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Desktops)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Desktop_Identity");

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Desktops)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Desktop_Category");

            builder.Property(e => e.MAC)
                .HasColumnType("varchar(255)");

            builder.Property(e => e.RAM)
                .HasColumnType("varchar(255)");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");

        }
    }
}
