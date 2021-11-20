using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configuration
{
    public class LaptopConfiguraton : IEntityTypeConfiguration<Laptop>
    {
        public void Configure(EntityTypeBuilder<Laptop> builder)
        {
            builder.ToTable(nameof(Laptop));

            builder.HasKey(e => e.AssetTag)
                .HasName("PK_Laptop_Asset");

            builder.Property(e => e.SerialNumber)
                .HasColumnType("varchar(255)");

            builder.HasOne(e => e.Type)
                .WithMany(d => d.Laptops)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Laptop_Type");

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Laptops)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Laptop_Identity");

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Laptops)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Laptop_Category");

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
