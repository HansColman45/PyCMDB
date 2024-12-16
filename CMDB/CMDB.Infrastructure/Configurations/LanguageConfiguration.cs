using CMDB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CMDB.Infrastructure.Configurations
{
    class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable(nameof(Language));

            builder.HasKey(e => e.Code)
                .HasName("PK_Language");

            builder.Property(e => e.Description)
                .HasColumnType("varchar(255)")
                .IsRequired();
        }
    }
}
