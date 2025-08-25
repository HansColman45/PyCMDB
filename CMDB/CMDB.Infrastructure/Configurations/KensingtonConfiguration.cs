using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CMDB.Infrastructure.Configurations
{
    class KensingtonConfiguration : IEntityTypeConfiguration<Kensington>
    {
        public void Configure(EntityTypeBuilder<Kensington> builder)
        {
            builder.ToTable(nameof(Kensington));

            builder.HasKey(e => e.KeyID)
                .HasName("PK_Kensington")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.HasOne(e => e.Type)
                .WithMany(d => d.Kensingtons)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .HasConstraintName("FK_Kensington_Type")
                .IsRequired();

            builder.HasOne(e => e.Device)
                .WithOne(d => d.Kensington)
                .HasForeignKey<Kensington>(e => e.AssetTag)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Key_Device")
                .IsRequired(false);

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Kensingtons)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .HasConstraintName("FK_Kensington_Category")
                .IsRequired();

            builder.Property(e => e.SerialNumber)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(e => e.HasLock)
                .IsRequired();

            builder.Property(e => e.AmountOfKeys)
                .IsRequired();

            builder.HasOne(e => e.LastModifiedAdmin)
                .WithMany(p => p.Kensingtons)
                .HasForeignKey(e => e.LastModifiedAdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Kensington_LastModifiedAdmin");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
