using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wolds.Hr.Api.Domain;

namespace Wolds.Hr.Api.Data.Context.Configuration;

internal sealed class ImportEmployeeFailedErrorHistoryConfiguration : IEntityTypeConfiguration<ImportEmployeeFailedErrorHistory>
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