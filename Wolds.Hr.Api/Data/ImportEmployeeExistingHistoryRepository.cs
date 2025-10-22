using Microsoft.EntityFrameworkCore;
using Wolds.Hr.Api.Data.Context;
using Wolds.Hr.Api.Data.Interfaces;
using Wolds.Hr.Api.Domain;

namespace Wolds.Hr.Api.Data;

internal sealed class ImportEmployeeExistingHistoryRepository(WoldsHrDbContext woldsHrDbContext) : IImportEmployeeExistingHistoryRepository
{
    public void Add(ImportEmployeeExistingHistory employee)
    {
        employee.Created = DateOnly.FromDateTime(DateTime.Now);

        woldsHrDbContext.ImportEmployeesExistingHistory.Add(employee);
    }

    public async Task<(List<ImportEmployeeExistingHistory>, int)> GetAsync(Guid id, int page, int pageSize)
    {
        var baseQuery = woldsHrDbContext.ImportEmployeesExistingHistory
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
}