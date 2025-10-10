using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context.Configuration;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context;

internal sealed class WoldsHrDbContext(DbContextOptions<WoldsHrDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<ImportEmployeeExistingHistory> ImportEmployeesExistingHistory { get; set; }
    public DbSet<ImportEmployeeFailedHistory> ImportEmployeesFailedHistory { get; set; }
    public DbSet<ImportEmployeeFailedErrorHistory> ImportEmployeesFailedErrorHistory { get; set; }
    public DbSet<ImportEmployeeHistory> ImportEmployeesHistory { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new ImportEmployeeHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new ImportEmployeeExistingHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new ImportEmployeeFailedHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new ImportEmployeeFailedErrorHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    }
}