using Microsoft.EntityFrameworkCore;
using Wolds.Hr.Api.Data.Context;
using Wolds.Hr.Api.Data.Interfaces;
using Wolds.Hr.Api.Domain;

namespace Wolds.Hr.Api.Data;

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
                                .Include(e => e.Errors)
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