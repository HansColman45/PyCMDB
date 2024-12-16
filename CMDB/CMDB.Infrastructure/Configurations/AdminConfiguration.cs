using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable(nameof(Admin));

            builder.HasKey(e => e.AdminId)
                .HasName("PK_Admin")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.Property(e => e.Level)
                .IsRequired();

            builder.Property(e => e.DateSet)
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.HasOne(e => e.Account)
                .WithMany(d => d.Admins)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Admin_Account")
                .IsRequired();

            builder.Property(e => e.Password)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.HasOne(e => e.LastModifiedAdmin)
                .WithMany(p => p.Admins)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Admin_LastModiefiedAdmin");

            builder.Property(e => e.active)
               .IsRequired()
               .HasMaxLength(1)
               .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
