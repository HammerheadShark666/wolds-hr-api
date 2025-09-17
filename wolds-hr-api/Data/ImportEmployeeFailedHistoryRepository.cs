using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class ImportEmployeeFailedHistoryRepository(WoldsHrDbContext context) : IImportEmployeeFailedHistoryRepository
{
    private readonly WoldsHrDbContext _context = context;

    public ImportEmployeeFailedHistory Add(ImportEmployeeFailedHistory employee)
    {
        _context.ImportEmployeesFailedHistory.Add(employee);

        return employee;
    }

    public async Task<List<ImportEmployeeFailedHistory>> GetAsync(Guid id, int page, int pageSize)
    {
        return await _context.ImportEmployeesFailedHistory
                            .Where(e => e.ImportEmployeeHistoryId.Equals(id))
                            .OrderBy(e => e.Employee)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .AsNoTracking()
                            .ToListAsync();
    }

    public async Task<int> CountAsync(Guid id)
    {
        return await _context.ImportEmployeesFailedHistory.Where(e => e.ImportEmployeeHistoryId.Equals(id)).CountAsync();
    }
}