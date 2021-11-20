using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CMDB.Infrastructure.Configuration
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
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Kensington_Type");

            builder.HasOne(e => e.Laptop)
                .WithMany(d => d.Keys)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Key_Laptop");

            builder.HasOne(e => e.Desktop)
                .WithMany(d => d.Keys)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Key_Deskop");

            builder.HasOne(e => e.Docking)
                .WithMany(d => d.Keys)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Key_Docking");

            builder.HasOne(e => e.Screen)
                .WithMany(d => d.Keys)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Key_Screen");

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Kensingtons)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Kensington_Category");

            builder.Property(e => e.SerialNumber)
                .HasColumnType("varchar(255)");

            builder.Property(e => e.HasLock);

            builder.Property(e => e.AmountOfKeys);

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
