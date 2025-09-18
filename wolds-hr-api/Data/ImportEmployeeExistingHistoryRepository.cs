using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class ImportEmployeeExistingHistoryRepository(WoldsHrDbContext context) : IImportEmployeeExistingHistoryRepository
{
    private readonly WoldsHrDbContext _context = context;

    public ImportEmployeeExistingHistory Add(ImportEmployeeExistingHistory employee)
    {
        employee.Created = DateOnly.FromDateTime(DateTime.Now);

        _context.ImportEmployeesExistingHistory.Add(employee);

        return employee;
    }

    public async Task<List<ImportEmployeeExistingHistory>> GetAsync(Guid id, int page, int pageSize)
    {
        return await _context.ImportEmployeesExistingHistory
                            .Where(e => e.ImportEmployeeHistoryId.Equals(id))
                            .OrderBy(e => e.Surname)
                                .ThenBy(e => e.FirstName)
                            .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                            .AsNoTracking()
                            .ToListAsync();
    }

    public async Task<int> CountAsync(Guid id)
    {
        return await _context.ImportEmployeesExistingHistory.Where(e => e.ImportEmployeeHistoryId.Equals(id)).CountAsync();
    }
}