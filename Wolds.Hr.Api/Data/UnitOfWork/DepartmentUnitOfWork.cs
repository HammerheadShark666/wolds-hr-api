using Wolds.Hr.Api.Data.Context;
using Wolds.Hr.Api.Data.Interfaces;
using Wolds.Hr.Api.Data.UnitOfWork.Interfaces;

namespace Wolds.Hr.Api.Data.UnitOfWork;

internal sealed class DepartmentUnitOfWork(IDepartmentRepository departmentRepository, WoldsHrDbContext dbContext) : IDepartmentUnitOfWork
{
    public IDepartmentRepository Department { get; } = departmentRepository;

    private readonly WoldsHrDbContext _dbContext = dbContext;

    public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
}
