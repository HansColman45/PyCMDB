using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Infrastructure.Configurations
{
    class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable("asset");

            builder.HasDiscriminator()
                .HasValue<Laptop>(nameof(Laptop))
                .HasValue<Desktop>(nameof(Desktop))
                .HasValue<Screen>(nameof(Screen))
                .HasValue<Token>(nameof(Token))
                .HasValue<Docking>(nameof(Docking));

            builder.HasKey(e => e.AssetTag)
                .HasName("PK_Device_AssetTag");

            builder.Property(e => e.SerialNumber)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.HasOne(e => e.Type)
                .WithMany(d => d.Devices)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Device_Type")
                .IsRequired(false);

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Devices)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Device_Identity")
                .IsRequired(false); 

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Devices)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Device_Category")
                .IsRequired(false);

            builder.HasOne(e => e.LastModifiedAdmin)
                .WithMany(p => p.Devices)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Device_LastModfiedAdmin");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
