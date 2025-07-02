using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class ExistingEmployeeRepository(AppDbContext context) : IExistingEmployeeRepository
{
    private readonly AppDbContext _context = context;

    public ExistingEmployee Add(ExistingEmployee existingEmployee)
    {
        existingEmployee.Created = DateOnly.FromDateTime(DateTime.Now);

        _context.ExistingEmployees.Add(existingEmployee);
        _context.SaveChangesAsync();

        return existingEmployee;
    }
}