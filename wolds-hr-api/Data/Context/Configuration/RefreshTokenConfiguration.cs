using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context.Configuration;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("WOLDS_HR_RefreshToken");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(u => u.Token)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(u => u.Expires)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(u => u.Created)
            .IsRequired()
            .HasColumnType("datetime");

        builder.HasOne(rt => rt.Account)
           .WithMany(a => a.RefreshTokens)
           .HasForeignKey(rt => rt.AccountId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}