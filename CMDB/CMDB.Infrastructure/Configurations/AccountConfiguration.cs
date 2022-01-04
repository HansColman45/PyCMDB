using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable(nameof(Account));

            builder.HasKey(e => e.AccID)
                .HasName("PK_Account")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.Property(e => e.UserID)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.HasOne(e => e.Type)
                .WithMany(p => p.Accounts)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Account_Type")
                .IsRequired();

            builder.HasOne(e => e.Application)
                .WithMany(p => p.Accounts)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Account_Application")
                .IsRequired();

            builder.HasOne(e => e.LastModfiedAdmin)
                .WithMany(p => p.Accounts)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .HasConstraintName("FK_Account_LastModifiedAdmin")
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
