using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable(nameof(Menu));

            builder.HasKey(e => e.MenuId)
                .HasName("PK_Menu")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.HasOne(e => e.Parent)
                .WithMany(d => d.Children)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(e => e.ParentId)
                .HasConstraintName("FK_Menu_Menu");

            builder.Property(e => e.Label)
                .HasColumnType("varchar(255)");

            builder.Property(e => e.URL)
                .IsRequired(false)
                .HasColumnType("varchar(255)");
        }
    }
}
