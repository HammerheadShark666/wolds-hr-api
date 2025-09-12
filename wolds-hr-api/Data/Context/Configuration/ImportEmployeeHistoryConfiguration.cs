using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context.Configuration;

public class ImportEmployeeHistoryConfiguration : IEntityTypeConfiguration<ImportEmployeeHistory>
{
    public void Configure(EntityTypeBuilder<ImportEmployeeHistory> builder)
    {
        builder.ToTable("WOLDS_HR_ImportEmployeeHistory");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(e => e.Date)
               .HasColumnType("datetime");

        builder.HasMany(x => x.ImportedEmployees);

        builder.HasMany(x => x.ExistingEmployees)
               .WithOne()
               .HasForeignKey(x => x.ImportEmployeeHistoryId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.FailedEmployees)
               .WithOne()
               .HasForeignKey(x => x.ImportEmployeeHistoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}