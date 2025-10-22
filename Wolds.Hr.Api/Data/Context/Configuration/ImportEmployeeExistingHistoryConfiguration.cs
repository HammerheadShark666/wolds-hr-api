using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wolds.Hr.Api.Domain;
using Wolds.Hr.Api.Library.Converters;

namespace Wolds.Hr.Api.Data.Context.Configuration;

internal sealed class ImportEmployeeExistingHistoryConfiguration : IEntityTypeConfiguration<ImportEmployeeExistingHistory>
{
    public void Configure(EntityTypeBuilder<ImportEmployeeExistingHistory> builder)
    {
        builder.ToTable("WOLDS_HR_ImportEmployeeExistingHistory");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(u => u.Surname)
               .IsRequired()
               .HasMaxLength(25);

        builder.Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(25);

        builder.Property(e => e.DateOfBirth)
               .HasConversion<DateOnlyConverter, DateOnlyComparer>()
               .HasColumnType("date");

        builder.Property(u => u.Email)
               .HasMaxLength(250);

        builder.Property(u => u.PhoneNumber)
              .HasMaxLength(25);
    }
}