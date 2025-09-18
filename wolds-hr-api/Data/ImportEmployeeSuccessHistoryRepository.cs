using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class ImportEmployeeSuccessHistoryRepository(WoldsHrDbContext context) : IImportEmployeeSuccessHistoryRepository
{
    private readonly WoldsHrDbContext _context = context;

    public async Task<(List<Employee>, int)> GetAsync(Guid id, int page, int pageSize)
    {
        var baseQuery = _context.Employees
                                .Where(e => e.ImportEmployeeHistoryId == id)
                                .AsNoTracking();

        var totalEmployees = await baseQuery.CountAsync();

        var employees = await baseQuery
                                .OrderBy(e => e.Surname)
                                    .ThenBy(e => e.FirstName)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

        return (employees, totalEmployees);
    }

    public async Task<int> CountAsync(Guid id)
    {
        return await _context.Employees.Where(e => e.ImportEmployeeHistoryId.Equals(id)).CountAsync();
    }
}