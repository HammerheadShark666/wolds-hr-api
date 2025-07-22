using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context.Configuration;

public class EmployeeImportConfiguration : IEntityTypeConfiguration<EmployeeImport>
{
    public void Configure(EntityTypeBuilder<EmployeeImport> builder)
    {
        builder.ToTable("Employee");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(e => e.Date)
               .HasColumnType("datetime");

        builder.HasMany(ei => ei.Employees)
              .WithOne()
              .HasForeignKey("EmployeeImportId")
              .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ei => ei.ExistingEmployees)
               .WithOne()
               .HasForeignKey("EmployeeImportId")
               .OnDelete(DeleteBehavior.Restrict);
    }
}