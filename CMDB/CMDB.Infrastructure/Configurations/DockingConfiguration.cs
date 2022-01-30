using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configuration
{
    public class DockingConfiguraton : IEntityTypeConfiguration<Docking>
    {
        public void Configure(EntityTypeBuilder<Docking> builder)
        {
            builder.HasBaseType<Device>();
        }
    }
}
