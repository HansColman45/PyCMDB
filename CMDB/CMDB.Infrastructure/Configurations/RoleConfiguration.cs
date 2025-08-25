using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role));

            builder.HasKey(e => e.RoleId)
                .HasName("PK_Role")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(e => e.Description)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.HasOne(e => e.Type)
                .WithMany(d => d.Roles)
                .HasConstraintName("FK_Role_Type")
                .OnDelete(DeleteBehavior.ClientNoAction)
                .IsRequired();

            builder.HasOne(e => e.LastModifiedAdmin)
                .WithMany(p => p.Roles)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Role_LastModifiedAdmin");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
