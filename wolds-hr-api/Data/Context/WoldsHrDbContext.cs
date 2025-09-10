using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context;

public class WoldsHrDbContext(DbContextOptions<WoldsHrDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<ExistingEmployee> ExistingEmployees { get; set; }
    public DbSet<EmployeeImport> EmployeeImports { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.EmployeeImport)
            .WithMany(i => i.Employees)
            .HasForeignKey(e => e.EmployeeImportId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ExistingEmployee>()
            .HasOne(e => e.EmployeeImport)
            .WithMany(i => i.ExistingEmployees)
            .HasForeignKey(e => e.EmployeeImportId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasOne<Department>(s => s.Department);
    }
}