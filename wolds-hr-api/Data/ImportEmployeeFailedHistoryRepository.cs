using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

internal sealed class ImportEmployeeFailedHistoryRepository(WoldsHrDbContext woldsHrDbContext) : IImportEmployeeFailedHistoryRepository
{
    public void Add(ImportEmployeeFailedHistory employee)
    {
        woldsHrDbContext.ImportEmployeesFailedHistory.Add(employee);
    }

    public async Task<(List<ImportEmployeeFailedHistory>, int)> GetAsync(Guid id, int page, int pageSize)
    {
        var baseQuery = woldsHrDbContext.ImportEmployeesFailedHistory
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