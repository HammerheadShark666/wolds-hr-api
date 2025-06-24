using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class EmployeeImportRepository(AppDbContext context) : IEmployeeImportRepository
{
    private readonly AppDbContext _context = context;

    public List<EmployeeImport> Get()
    {
        return [.. _context.EmployeeImports.OrderByDescending(a => a.Date)];
    }

    public EmployeeImport Add()
    {
        EmployeeImport employeeImport = new()
        {
            Date = DateTime.Now
        };

        _context.EmployeeImports.Add(employeeImport);
        _context.SaveChangesAsync();

        return employeeImport;
    }
}
