using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;

namespace wolds_hr_api.Data.Context.Configuration;

public class ExistingEmployeeConfiguration : IEntityTypeConfiguration<ExistingEmployee>
{
    public void Configure(EntityTypeBuilder<ExistingEmployee> builder)
    {
        builder.ToTable("Employee");

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

        builder.HasOne(e => e.EmployeeImport)
               .WithMany()
               .HasForeignKey(e => e.EmployeeImportId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}