using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configuration
{
    public class DockingConfiguraton : IEntityTypeConfiguration<Docking>
    {
        public void Configure(EntityTypeBuilder<Docking> builder)
        {
            builder.ToTable(nameof(Docking));

            builder.HasKey(e => e.AssetTag)
                .HasName("PK_Docking_Asset");

            builder.Property(e => e.SerialNumber)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.HasOne(e => e.Type)
                .WithMany(d => d.Dockings)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Docking_Type")
                .IsRequired();

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Dockings)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Docking_Identity");

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Dockings)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Docking_Category")
                .IsRequired();

            builder.HasOne(e => e.LastModfiedAdmin)
                .WithMany(p => p.Dockings)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Docking_LastModifiedAdmin");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
