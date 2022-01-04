using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    public class RolePermConfiguration : IEntityTypeConfiguration<RolePerm>
    {
        public void Configure(EntityTypeBuilder<RolePerm> builder)
        {
            builder.ToTable(nameof(RolePerm));

            builder.HasKey(e => e.Id)
                .HasName("PK_RolePerm")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.Property(e => e.Level);

            builder.HasOne(e => e.Menu)
                .WithMany(d => d.Permissions)
                .HasForeignKey(e => e.MenuId)
                .HasConstraintName("FK_RolePerm_Menu")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(e => e.Permission)
                .WithMany(d => d.Roles)
                .HasForeignKey(e => e.PermissionId)
                .HasConstraintName("FK_RolePerm_Permission")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(e => e.LastModifiedAdmin)
                .WithMany(p => p.RolePerms)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_RolePerm_LastModifiedAdmin")
                .IsRequired(false);
        }
    }
}
