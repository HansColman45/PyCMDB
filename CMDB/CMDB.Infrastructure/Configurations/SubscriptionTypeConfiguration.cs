using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    class SubscriptionTypeConfiguration : IEntityTypeConfiguration<SubscriptionType>
    {
        public void Configure(EntityTypeBuilder<SubscriptionType> builder)
        {
            builder.ToTable(nameof(SubscriptionType));

            builder.HasKey(e => e.Id)
                .HasName("PK_SubscriptionType");

            builder.Property(e => e.Type);

            builder.Property(e => e.Description);

            builder.Property(e => e.Provider);

            builder.HasOne(e => e.Category)
                .WithMany(d => d.SubscriptionTypes)
                .HasForeignKey(e => e.AssetCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AssetType_Category")
                .IsRequired();

            builder.HasOne(e => e.LastModifiedAdmin)
                .WithMany(p => p.SubscriptionTypes)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_SubscriptionType_LastModifiedAdmin");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
