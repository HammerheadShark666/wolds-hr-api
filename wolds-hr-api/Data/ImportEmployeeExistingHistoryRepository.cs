using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class ImportEmployeeExistingHistoryRepository(WoldsHrDbContext context) : IImportEmployeeExistingHistoryRepository
{
    private readonly WoldsHrDbContext _context = context;

    public void Add(ImportEmployeeExistingHistory employee)
    {
        employee.Created = DateOnly.FromDateTime(DateTime.Now);

        _context.ImportEmployeesExistingHistory.Add(employee);
    }

    public async Task<(List<ImportEmployeeExistingHistory>, int)> GetAsync(Guid id, int page, int pageSize)
    {
        var baseQuery = _context.ImportEmployeesExistingHistory
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