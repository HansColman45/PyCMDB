using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    public class IdentityAccountInfoConfiguration : IEntityTypeConfiguration<IdentityAccountInfo>
    {
        public void Configure(EntityTypeBuilder<IdentityAccountInfo> builder)
        {
            builder.HasNoKey();
        }
    }
}
