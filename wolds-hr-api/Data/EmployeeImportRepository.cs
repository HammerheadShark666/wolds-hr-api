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

    public List<Employee> GetImportedEmployees(int id, int page, int pageSize)
    {
        var departments = _context.Departments.ToList();
        var employees = _context.Employees.ToList();

        var result = (from e in employees
                      join d in departments
                      on e.DepartmentId equals d.Id into deptGroup
                      from dept in deptGroup.DefaultIfEmpty()
                      where e.EmployeeImportId.Equals(id)
                      select new Employee()
                      {
                          Id = e.Id,
                          Surname = e.Surname,
                          FirstName = e.FirstName,
                          DateOfBirth = e.DateOfBirth,
                          HireDate = e.HireDate,
                          Email = e.Email,
                          PhoneNumber = e.PhoneNumber,
                          Photo = e.Photo,
                          Created = e.Created,
                          DepartmentId = dept != null ? dept.Id : 0,
                          Department = dept ?? null,
                          EmployeeImportId = e.EmployeeImportId
                      })
                     .OrderBy(a => a.Surname).ThenBy(a => a.FirstName)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToList();

        return result;
    }

    public int CountImportedEmployees(int id)
    {
        return _context.Employees.Where(e => e.EmployeeImportId == id).Count();
    }
}