using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context.Configuration;

public class ImportEmployeeFailedErrorHistoryConfiguration : IEntityTypeConfiguration<ImportEmployeeFailedErrorHistory>
{
    public void Configure(EntityTypeBuilder<ImportEmployeeFailedErrorHistory> builder)
    {
        builder.ToTable("WOLDS_HR_ImportEmployeeFailedErrorHistory");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.Error)
               .IsRequired();
    }
}