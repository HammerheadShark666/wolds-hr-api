using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class ImportEmployeeSuccessHistoryRepository(WoldsHrDbContext context) : IImportEmployeeSuccessHistoryRepository
{
    private readonly WoldsHrDbContext _context = context;

    public async Task<List<Employee>> GetAsync(Guid id, int page, int pageSize)
    {
        return await _context.Employees
                            .Where(e => e.ImportEmployeeHistoryId.Equals(id))
                            .OrderBy(e => e.Surname)
                            .ThenBy(e => e.FirstName)
                            .Select(e => new Employee
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

    public async Task<int> CountAsync(Guid id)
    {
        return await _context.Employees.Where(e => e.ImportEmployeeHistoryId.Equals(id)).CountAsync();
    }
}