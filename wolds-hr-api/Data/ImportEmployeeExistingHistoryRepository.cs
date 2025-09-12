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
        _context.SaveChangesAsync();

        return employee;
    }

    public async Task<List<ImportEmployeeExistingHistory>> GetImportedExistingEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        return await _context.ImportEmployeesExistingHistory
                            .Where(e => e.ImportEmployeeHistoryId.Equals(id))
                            .OrderBy(e => e.Surname)
                            .ThenBy(e => e.FirstName)
                            .Select(e => new ImportEmployeeExistingHistory
                            {
                                Id = e.Id,
                                Surname = e.Surname,
                                FirstName = e.FirstName,
                                DateOfBirth = e.DateOfBirth,
                                Email = e.Email,
                                PhoneNumber = e.PhoneNumber,
                                Created = e.Created,
                                ImportEmployeeHistoryId = e.ImportEmployeeHistoryId
                            })
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
    }

    public async Task<int> CountImportedExistingEmployeesHistoryAsync(Guid id)
    {
        return await _context.ImportEmployeesExistingHistory.Where(e => e.ImportEmployeeHistoryId.Equals(id)).CountAsync();
    }
}