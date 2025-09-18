using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Data;

public class ImportEmployeeHistoryRepository(WoldsHrDbContext context) : IImportEmployeeHistoryRepository
{
    private readonly WoldsHrDbContext _context = context;
    public async Task<List<ImportEmployeeHistory>> GetAsync()
    {
        return await _context.ImportEmployeesHistory
                             .OrderByDescending(a => a.Date)
                             .AsNoTracking()
                             .ToListAsync();
    }
    public void Add(ImportEmployeeHistory importEmployeeHistory)
    {
        _context.ImportEmployeesHistory.Add(importEmployeeHistory);
    }

    public async Task DeleteAsync(Guid id)
    {
        var employeeImport = await _context.ImportEmployeesHistory.FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (employeeImport != null)
        {
            var import = await _context.ImportEmployeesHistory
                            .Include(i => i.ImportedEmployees)
                            .Include(i => i.ExistingEmployees)
                            .FirstOrDefaultAsync(i => i.Id.Equals(id));

            if (import != null)
            {
                _context.Employees.RemoveRange(import.ImportedEmployees);
                _context.ImportEmployeesExistingHistory.RemoveRange(import.ExistingEmployees);
                _context.ImportEmployeesHistory.Remove(import);
            }
        }
        else
            throw new ImportEmployeeHistoryNotFoundException("ImportEmployee not found");
    }
}