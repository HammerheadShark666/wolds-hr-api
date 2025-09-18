using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Data.UnitOfWork.Interfaces;

namespace wolds_hr_api.Data.UnitOfWork;

public class EmployeeUnitOfWork(IEmployeeRepository employeeRepository, WoldsHrDbContext dbContext) : IEmployeeUnitOfWork
{
    public IEmployeeRepository Employee { get; } = employeeRepository;

    private readonly WoldsHrDbContext _dbContext = dbContext;

    public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
}
