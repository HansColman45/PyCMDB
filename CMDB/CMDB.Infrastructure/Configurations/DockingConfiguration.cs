using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    public class DockingConfiguraton : IEntityTypeConfiguration<Docking>
    {
        public void Configure(EntityTypeBuilder<Docking> builder)
        {
            builder.HasBaseType<Device>();
        }
    }
}
