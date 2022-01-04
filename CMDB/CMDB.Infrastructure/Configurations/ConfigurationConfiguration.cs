using Microsoft.EntityFrameworkCore;
using System;
using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMDB.Infrastructure.Configurations
{
    class ConfigurationConfiguration : IEntityTypeConfiguration<CMDB.Domain.Entities.Configuration>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Configuration> builder)
        {
            builder.ToTable(nameof(CMDB.Domain.Entities.Configuration));

            builder.Property(e => e.Code)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(e => e.SubCode)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(e => e.CFN_Date)
                .IsRequired(false)
                .HasColumnType("datetime2(0)");

            builder.Property(e => e.CFN_Number)
                .IsRequired(false);

            builder.Property(e => e.CFN_Tekst)
                .IsRequired(false)
                .HasColumnType("varchar(255)");

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasColumnType("varchar(255)");

            builder.HasKey(e => new { e.Code, e.SubCode });
        }
    }
}
