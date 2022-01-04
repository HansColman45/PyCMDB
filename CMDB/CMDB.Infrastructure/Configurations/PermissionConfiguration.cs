using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(nameof(Permission));

            builder.HasKey(e => e.Id)
                .HasName("PK_PPermission");

            builder.Property(e => e.Id)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.Property(e => e.Rights)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(e => e.Description)
                .HasColumnType("varchar(255)");

            builder.HasOne(e => e.LastModifiedAdmin)
                .WithMany(p => p.Permissions)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Permission_CreatedAdmin");
        }
    }
}
