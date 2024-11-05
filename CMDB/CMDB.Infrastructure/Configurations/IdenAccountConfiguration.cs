using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CMDB.Infrastructure.Configuration
{
    class IdenAccountConfiguration : IEntityTypeConfiguration<IdenAccount>
    {
        public void Configure(EntityTypeBuilder<IdenAccount> builder)
        {
            builder.ToTable(nameof(IdenAccount));

            builder.HasKey(e => e.ID)
                .HasName("PK_IdenAccount")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Accounts)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_IdenAccount_Identity")
                .IsRequired();

            builder.HasOne(e => e.Account)
                .WithMany(d => d.Identities)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_IdenAccount_Account")
                .IsRequired();

            builder.Property(e => e.ValidFrom)
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(e => e.ValidUntil)
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.HasOne(e => e.LastModifiedAdmin)
                 .WithMany(p => p.IdenAccounts)
                 .HasForeignKey(e => e.LastModifiedAdminId)
                 .OnDelete(DeleteBehavior.SetNull)
                 .HasConstraintName("FK_IdenAccount_LastModifiedAdmin");

            builder.HasIndex(e => new { e.AccountId, e.IdentityId, e.ValidFrom, e.ValidUntil }).IsUnique();
        }
    }
}
