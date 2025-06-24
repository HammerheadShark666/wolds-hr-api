using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeImport> EmployeeImports { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>()
            .HasOne<Department>(s => s.Department);

        var employees = DefaultData.Employees.GetEmployeeDefaultData().Concat(DefaultData.Employees.GetRandomEmployeeDefaultData());

        modelBuilder.Entity<Account>().HasData(DefaultData.Accounts.GetAccountDefaultData());
        modelBuilder.Entity<Department>().HasData(DefaultData.Departments.GetDepartmentDefaultData());
        modelBuilder.Entity<Employee>().HasData(employees);
    }
}