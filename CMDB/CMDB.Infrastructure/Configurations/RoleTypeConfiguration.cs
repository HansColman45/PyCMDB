using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    public class RoleTypeConfiguration : IEntityTypeConfiguration<RoleType>
    {
        public void Configure(EntityTypeBuilder<RoleType> builder)
        {
            builder.HasBaseType<GeneralType>();
        }
    }
}
