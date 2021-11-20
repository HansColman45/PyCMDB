using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CMDB.Infrastructure.Configuration
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
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Account");

            builder.HasOne(e => e.AccountType)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.AccountTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_AccounType");

            builder.HasOne(e => e.Admin)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.AdminId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Admin");

            builder.HasOne(e => e.Application)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Application");

            builder.HasOne(e => e.AssetType)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.AssetTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_AssetType");

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.AssetCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Category");

            builder.HasOne(e => e.Desktop)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.DesktopAssetTag)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Desktop");

            builder.HasOne(e => e.Docking)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.DockingAssetTag)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Docking");

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Identity");

            builder.HasOne(e => e.IdentityType)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.IdentityTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_IdentityType");

            builder.HasOne(e => e.Kensington)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.KensingtonId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_KensingTone");

            builder.HasOne(e => e.Laptop)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.LaptopAssetTag)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Laptop");

            builder.HasOne(e => e.Menu)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.MenuId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Menu");

            builder.HasOne(e => e.Mobile)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.MobileId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Mobile");

            builder.HasOne(e => e.Screen)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.ScreenAssetTag)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Screen");

            builder.HasOne(e => e.Permission)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Permission");

            builder.HasOne(e => e.Role)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Role");

            builder.HasOne(e => e.RoleType)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.RoleTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_RoleType");

            builder.HasOne(e => e.Subscription)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.SubsriptionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Subscription");

            builder.HasOne(e => e.SubscriptionType)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.SubscriptionTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_SubscriptionType");

            builder.HasOne(e => e.Token)
                .WithMany(d => d.Logs)
                .HasForeignKey(e => e.TokenAssetTag)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Log_Token");
        }
    }
}
