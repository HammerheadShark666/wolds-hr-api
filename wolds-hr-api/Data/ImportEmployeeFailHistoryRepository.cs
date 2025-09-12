using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class ImportEmployeeFailHistoryRepository(WoldsHrDbContext context) : IImportEmployeeFailHistoryRepository
{
    private readonly WoldsHrDbContext _context = context;

    public async Task<ImportEmployeeFailHistory> AddAsync(string employee, Guid importEmployeeHistoryId, string[] errors)
    {
        List<ImportEmployeeFailErrorHistory> importEmployeeFailErrorHistory = [];

        foreach (var error in errors)
        {
            importEmployeeFailErrorHistory.Add(new ImportEmployeeFailErrorHistory { Error = error, ImportEmployeeFailHistoryId = importEmployeeHistoryId });
        }

        ImportEmployeeFailHistory employeeImportHistory = new()
        {
            Employee = employee,
            ImportEmployeeHistoryId = importEmployeeHistoryId,
            Errors = importEmployeeFailErrorHistory
        };

        _context.ImportEmployeesFailHistory.Add(employeeImportHistory);
        await _context.SaveChangesAsync();

        return employeeImportHistory;
    }

    public async Task<List<ImportEmployeeFailHistory>> GetImportedFailedEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        return await _context.ImportEmployeesFailHistory
                            .Where(e => e.ImportEmployeeHistoryId.Equals(id))
                            .OrderBy(e => e.Employee)
                            .Select(e => new ImportEmployeeFailHistory
                            {
                                Id = e.Id,
                                Errors = e.Errors,
                                Employee = e.Employee,
                                ImportEmployeeHistoryId = e.ImportEmployeeHistoryId
                            })
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
    }

    public async Task<int> CountImportedFailedEmployeesHistoryAsync(Guid id)
    {
        return await _context.ImportEmployeesFailHistory.Where(e => e.ImportEmployeeHistoryId.Equals(id)).CountAsync();
    }
}