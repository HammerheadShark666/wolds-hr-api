using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class ImportEmployeeFailedHistoryRepository(WoldsHrDbContext context) : IImportEmployeeFailedHistoryRepository
{
    private readonly WoldsHrDbContext _context = context;

    public void Add(ImportEmployeeFailedHistory employee)
    {
        _context.ImportEmployeesFailedHistory.Add(employee);
    }

    public async Task<(List<ImportEmployeeFailedHistory>, int)> GetAsync(Guid id, int page, int pageSize)
    {
        var baseQuery = _context.ImportEmployeesFailedHistory
                                .Where(e => e.ImportEmployeeHistoryId == id)
                                .AsNoTracking();

        var totalEmployees = await baseQuery.CountAsync();

        var employees = await baseQuery
                                .OrderBy(e => e.Employee)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

        return (employees, totalEmployees);
    }
}