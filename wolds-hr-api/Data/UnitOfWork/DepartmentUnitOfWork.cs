using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Data.UnitOfWork.Interfaces;

namespace wolds_hr_api.Data.UnitOfWork;

public class DepartmentUnitOfWork(IDepartmentRepository departmentRepository, WoldsHrDbContext dbContext) : IDepartmentUnitOfWork
{
    public IDepartmentRepository Department { get; } = departmentRepository;

    private readonly WoldsHrDbContext _dbContext = dbContext;

    public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
}
