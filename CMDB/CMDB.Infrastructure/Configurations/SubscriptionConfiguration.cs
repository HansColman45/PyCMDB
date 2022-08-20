using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable(nameof(Subscription));

            builder.HasKey(e => e.SubscriptionId)
                .HasName("PK_Subscription");

            builder.HasOne(e => e.SubscriptionType)
                .WithMany(e => e.Subscriptions)
                .HasForeignKey(e => e.SubsctiptionTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Subscription_Type")
                .IsRequired();

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Subscriptions)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Subscription_Identity");

            builder.HasOne(e => e.Mobile)
                .WithMany(d => d.Subscriptions)
                .HasForeignKey(e => e.MobileId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Subscription_Mobile");

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Subscriptions)
                .HasForeignKey(e => e.AssetCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Subscription_Category")
                .IsRequired();

            builder.Property(e => e.PhoneNumber);

            builder.HasOne(e => e.LastModfiedAdmin)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Subscription_LastModifiedAdmin");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
