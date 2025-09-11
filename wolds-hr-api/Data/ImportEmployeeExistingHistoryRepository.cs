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
}