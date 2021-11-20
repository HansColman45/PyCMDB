

using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    public class RAMConfiguration : IEntityTypeConfiguration<RAM>
    {
        public void Configure(EntityTypeBuilder<RAM> builder)
        {
            builder.ToTable(nameof(RAM));

            builder.HasKey(x => x.Id)
                .HasName("PK_RAM")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.Display)
                .IsRequired()
                .HasColumnType("varchar(255)");
        }
    }
}
