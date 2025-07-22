using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context.Configuration;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
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

        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeImportConfiguration());
        modelBuilder.ApplyConfiguration(new ExistingEmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

        var departments = DefaultData.Departments.GetDepartmentDefaultData();
        var employees = DefaultData.Employees.GetEmployeeDefaultData(departments).Concat(DefaultData.Employees.GetRandomEmployeeDefaultData(departments));

        modelBuilder.Entity<Account>().HasData(DefaultData.Accounts.GetAccountDefaultData());
        modelBuilder.Entity<Department>().HasData(departments);
        modelBuilder.Entity<Employee>().HasData(employees);
    }
}