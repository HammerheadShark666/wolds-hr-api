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
                            .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                            .AsNoTracking()
                            .ToListAsync();
    }

    public async Task<int> CountAsync(Guid id)
    {
        return await _context.Employees.Where(e => e.ImportEmployeeHistoryId.Equals(id)).CountAsync();
    }
}