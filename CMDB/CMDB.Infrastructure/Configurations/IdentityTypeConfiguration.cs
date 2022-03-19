using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configuration
{
    class IdentityTypeConfiguration : IEntityTypeConfiguration<IdentityType>
    {
        public void Configure(EntityTypeBuilder<IdentityType> builder)
        {
            builder.HasBaseType<GeneralType>();
        }
    }
}
