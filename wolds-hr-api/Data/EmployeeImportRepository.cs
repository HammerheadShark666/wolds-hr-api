using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Data;

public class EmployeeImportRepository(AppDbContext context) : IEmployeeImportRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<EmployeeImport>> GetAsync()
    {
        return await _context.EmployeeImports.OrderByDescending(a => a.Date).ToListAsync();
    }

    public async Task<EmployeeImport> AddAsync()
    {
        EmployeeImport employeeImport = new()
        {
            Date = DateTime.Now,
            Employees = [],
            ExistingEmployees = []
        };

        _context.EmployeeImports.Add(employeeImport);
        await _context.SaveChangesAsync();

        return employeeImport;
    }

    public async Task<List<Employee>> GetImportedEmployeesAsync(int id, int page, int pageSize)
    {
        return await (from e in _context.Employees
                      where e.EmployeeImportId == id
                      join d in _context.Departments on e.DepartmentId equals d.Id into deptGroup
                      from dept in deptGroup.DefaultIfEmpty() // LEFT JOIN
                      orderby e.Surname, e.FirstName
                      select new Employee
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
                          Department = dept,
                          EmployeeImportId = e.EmployeeImportId
                      }
                )
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }

    public async Task<List<ExistingEmployee>> GetImportedExistingEmployeesAsync(int id, int page, int pageSize)
    {
        return await _context.ExistingEmployees
                            .Where(e => e.EmployeeImportId == id)
                            .OrderBy(e => e.Surname)
                            .ThenBy(e => e.FirstName)
                            .Select(e => new ExistingEmployee
                            {
                                Id = e.Id,
                                Surname = e.Surname,
                                FirstName = e.FirstName,
                                DateOfBirth = e.DateOfBirth,
                                Email = e.Email,
                                PhoneNumber = e.PhoneNumber,
                                Created = e.Created,
                                EmployeeImportId = e.EmployeeImportId
                            })
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
    }

    public async Task<int> CountImportedEmployeesAsync(int id)
    {
        return await _context.Employees.Where(e => e.EmployeeImportId == id).CountAsync();
    }

    public async Task<int> CountImportedExistingEmployeesAsync(int id)
    {
        return await _context.ExistingEmployees.Where(e => e.EmployeeImportId == id).CountAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var employeeImport = await _context.EmployeeImports.FirstOrDefaultAsync(e => e.Id == id);
        if (employeeImport != null)
        {
            var import = await _context.EmployeeImports
                            .Include(i => i.Employees)
                            .Include(i => i.ExistingEmployees)
                            .FirstOrDefaultAsync(i => i.Id == id);

            if (import != null)
            {
                _context.Employees.RemoveRange(import.Employees);
                _context.ExistingEmployees.RemoveRange(import.ExistingEmployees);
                _context.EmployeeImports.Remove(import);

                await _context.SaveChangesAsync();
            }
        }
        else
            throw new EmployeeNotFoundException("employeeImport not found");
    }
}