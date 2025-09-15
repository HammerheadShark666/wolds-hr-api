using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class ImportEmployeeFailedHistoryRepository(WoldsHrDbContext context) : IImportEmployeeFailedHistoryRepository
{
    private readonly WoldsHrDbContext _context = context;

    public async Task<ImportEmployeeFailedHistory> AddAsync(ImportEmployeeFailedHistory employee)
    {
        _context.ImportEmployeesFailedHistory.Add(employee);
        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<List<ImportEmployeeFailedHistory>> GetAsync(Guid id, int page, int pageSize)
    {
        return await _context.ImportEmployeesFailedHistory
                            .Where(e => e.ImportEmployeeHistoryId.Equals(id))
                            .OrderBy(e => e.Employee)
                            .Select(e => new ImportEmployeeFailedHistory
                            {
                                Id = e.Id,
                                Employee = e.Employee,
                                ImportEmployeeHistoryId = e.ImportEmployeeHistoryId,
                                Errors = e.Errors
                            })
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
    }

    public async Task<int> CountAsync(Guid id)
    {
        return await _context.ImportEmployeesFailedHistory.Where(e => e.ImportEmployeeHistoryId.Equals(id)).CountAsync();
    }
}