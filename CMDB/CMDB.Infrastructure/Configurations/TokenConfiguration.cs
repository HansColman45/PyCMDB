using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configuration
{
    public class TokenConfiguraton : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.ToTable(nameof(Token));

            builder.HasKey(e => e.AssetTag)
                .HasName("PK_Token_Asset");

            builder.Property(e => e.SerialNumber)
                .HasColumnType("varchar(255)");

            builder.HasOne(e => e.Type)
                .WithMany(d => d.Tokens)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Token_Type");

            builder.HasOne(e => e.Identity)
                .WithMany(d => d.Tokens)
                .HasForeignKey(e => e.IdentityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Token_Identity");

            builder.HasOne(e => e.Category)
                .WithMany(d => d.Tokens)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Token_Category");

            builder.Property(e => e.active)
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(1);

            builder.Property(e => e.DeactivateReason)
                .HasColumnType("varchar(255)");
        }
    }
}
