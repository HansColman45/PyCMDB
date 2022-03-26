using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configuration
{
    public class DesktopConfiguraton : IEntityTypeConfiguration<Desktop>
    {
        public void Configure(EntityTypeBuilder<Desktop> builder)
        {
            builder.HasBaseType<Device>();

            builder.Property(e => e.MAC)
                .HasColumnType("varchar(255)")
                .HasColumnName("MAC");

            builder.Property(e => e.RAM)
                .HasColumnType("varchar(255)")
                .HasColumnName("RAM");
        }
    }
}
