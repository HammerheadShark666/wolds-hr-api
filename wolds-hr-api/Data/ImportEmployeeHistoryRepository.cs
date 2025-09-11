using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Data;

public class ImportEmployeeHistoryRepository(WoldsHrDbContext context) : IImportEmployeeHistoryRepository
{
    private readonly WoldsHrDbContext _context = context;

    public async Task<List<ImportEmployeeHistory>> GetAsync()
    {
        return await _context.ImportEmployeesHistory.OrderByDescending(a => a.Date).ToListAsync();
    }

    public async Task<ImportEmployeeHistory> AddAsync()
    {
        ImportEmployeeHistory employeeImportHistory = new()
        {
            Date = DateTime.Now,
            ImportedEmployees = [],
            ExistingEmployees = [],
            FailedEmployees = []
        };

        _context.ImportEmployeesHistory.Add(employeeImportHistory);
        await _context.SaveChangesAsync();

        return employeeImportHistory;
    }

    public async Task<List<Employee>> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        return await (from e in _context.Employees
                      where e.ImportEmployeeHistoryId.Equals(id)
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
                          DepartmentId = dept != null ? dept.Id : Guid.Empty,
                          Department = dept,
                          ImportEmployeeHistoryId = e.ImportEmployeeHistoryId
                      }
                )
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }

    public async Task<List<ImportEmployeeExistingHistory>> GetImportedExistingEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        return await _context.ImportEmployeesExistingHistory
                            .Where(e => e.ImportEmployeeHistoryId.Equals(id))
                            .OrderBy(e => e.Surname)
                            .ThenBy(e => e.FirstName)
                            .Select(e => new ImportEmployeeExistingHistory
                            {
                                Id = e.Id,
                                Surname = e.Surname,
                                FirstName = e.FirstName,
                                DateOfBirth = e.DateOfBirth,
                                Email = e.Email,
                                PhoneNumber = e.PhoneNumber,
                                Created = e.Created,
                                ImportEmployeeHistoryId = e.ImportEmployeeHistoryId
                            })
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
    }

    public async Task<int> CountImportedEmployeesHistoryAsync(Guid id)
    {
        return await _context.Employees.Where(e => e.ImportEmployeeHistoryId.Equals(id)).CountAsync();
    }

    public async Task<int> CountImportedExistingEmployeesHistoryAsync(Guid id)
    {
        return await _context.ImportEmployeesExistingHistory.Where(e => e.ImportEmployeeHistoryId.Equals(id)).CountAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var employeeImport = await _context.ImportEmployeesHistory.FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (employeeImport != null)
        {
            var import = await _context.ImportEmployeesHistory
                            .Include(i => i.ImportedEmployees)
                            .Include(i => i.ExistingEmployees)
                            .FirstOrDefaultAsync(i => i.Id.Equals(id));

            if (import != null)
            {
                _context.Employees.RemoveRange(import.ImportedEmployees);
                _context.ImportEmployeesExistingHistory.RemoveRange(import.ExistingEmployees);
                _context.ImportEmployeesHistory.Remove(import);

                await _context.SaveChangesAsync();
            }
        }
        else
            throw new ImportEmployeeHistoryNotFoundException("ImportEmployee not found");
    }
}