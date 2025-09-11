namespace wolds_hr_api.Data.Context.Configuration;

using global::wolds_hr_api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ImportEmployeeFailHistoryConfiguration : IEntityTypeConfiguration<ImportEmployeeFailHistory>
{
    public void Configure(EntityTypeBuilder<ImportEmployeeFailHistory> builder)
    {
        builder.ToTable("WOLDS_HR_ImportEmployeeFailHistory");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(u => u.Employee)
               .IsRequired()
               .HasMaxLength(500);

        builder.HasMany(x => x.Errors)
               .WithOne()
               .HasForeignKey(x => x.ImportEmployeeFailHistoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
