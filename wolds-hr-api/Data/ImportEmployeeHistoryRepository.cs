using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Library.Exceptions;

namespace wolds_hr_api.Data;

internal sealed class ImportEmployeeHistoryRepository(WoldsHrDbContext woldsHrDbContext) : IImportEmployeeHistoryRepository
{
    public async Task<List<ImportEmployeeHistory>> GetAsync()
    {
        return await woldsHrDbContext.ImportEmployeesHistory
                             .OrderByDescending(a => a.Date)
                             .AsNoTracking()
                             .ToListAsync();
    }
    public void Add(ImportEmployeeHistory importEmployeeHistory)
    {
        woldsHrDbContext.ImportEmployeesHistory.Add(importEmployeeHistory);
    }

    public async Task DeleteAsync(Guid id)
    {
        var employeeImport = await woldsHrDbContext.ImportEmployeesHistory.FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (employeeImport != null)
        {
            var import = await woldsHrDbContext.ImportEmployeesHistory
                            .Include(i => i.ImportedEmployees)
                            .Include(i => i.ExistingEmployees)
                            .FirstOrDefaultAsync(i => i.Id.Equals(id));

            if (import != null)
            {
                woldsHrDbContext.Employees.RemoveRange(import.ImportedEmployees);
                woldsHrDbContext.ImportEmployeesExistingHistory.RemoveRange(import.ExistingEmployees);
                woldsHrDbContext.ImportEmployeesHistory.Remove(import);
            }
        }
        else
            throw new ImportEmployeeHistoryNotFoundException();
    }
}