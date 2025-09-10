using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("WOLDS_HR_Account");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(u => u.Surname)
               .IsRequired()
               .HasMaxLength(25);

        builder.Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(25);

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(u => u.PasswordHash)
               .HasMaxLength(150);

        builder.Property(u => u.Role)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(u => u.VerificationToken)
               .HasMaxLength(150);

        builder.Property(u => u.Verified)
               .HasMaxLength(150);

        builder.Property(u => u.ResetToken)
               .HasMaxLength(150);

        builder.Property(u => u.ResetTokenExpires)
               .HasColumnType("datetime");

        builder.Property(u => u.Created)
               .HasColumnType("datetime");
    }
}