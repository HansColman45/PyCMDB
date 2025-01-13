using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CMDB.Infrastructure.Configurations
{
    class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable(nameof(Log));

            builder.HasKey(e => e.Id)
                .HasName("PK_Log");

            builder.Property(e => e.LogDate)
                .IsRequired();

            builder.Property(e => e.LogText)
                .IsRequired();

            builder.HasOne(e => e.Account)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Account");

            builder.HasOne(e => e.Type)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Type");

            builder.HasOne(e => e.Admin)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.AdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Admin");

            builder.HasOne(e => e.Application)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Application");

            builder.HasOne(e => e.AssetType)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.AssetTypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_AssetType");

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.AssetCategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Category");

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Identity");

            builder.HasOne(e => e.Kensington)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.KensingtonId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_KensingTone");

            builder.HasOne(e => e.Menu)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.MenuId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Menu");

            builder.HasOne(e => e.Mobile)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.MobileId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Mobile");

            builder.HasOne(e => e.Permission)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Permission");

            builder.HasOne(e => e.Role)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Role");

            builder.HasOne(e => e.Subscription)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.SubsriptionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_Subscription");

            builder.HasOne(e => e.SubscriptionType)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.SubscriptionTypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Log_SubscriptionType");

            builder.HasOne(e => e.Device)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.AssetTag)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Device_Asset");
        }
    }
}
