using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    public class GeneralTypeConfiguration : IEntityTypeConfiguration<GeneralType>
    {
        public void Configure(EntityTypeBuilder<GeneralType> builder)
        {
            builder.ToTable("Type");
            
            builder.HasKey(e => e.TypeId)
                .HasName("PK_Type")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.HasDiscriminator()
                .HasValue(nameof(AccountType))
                .HasValue(nameof(IdentityType))
                .HasValue(nameof(RoleType))
                .HasValue(nameof(GeneralType));

            builder.Property(e => e.Description)
                .HasColumnType("varchar(255)");

            builder.Property(e => e.Type)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.HasOne(e => e.LastModifiedAdmin)
                .WithMany(p => p.Types)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Type_LastModifiedAdmin");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
