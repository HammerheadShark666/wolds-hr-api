using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context.Configuration;

public class ImportEmployeeFailErrorHistoryConfiguration : IEntityTypeConfiguration<ImportEmployeeFailErrorHistory>
{
    public void Configure(EntityTypeBuilder<ImportEmployeeFailErrorHistory> builder)
    {
        builder.ToTable("WOLDS_HR_ImportEmployeeFailErrorHistory");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.Error)
               .IsRequired();
    }
}