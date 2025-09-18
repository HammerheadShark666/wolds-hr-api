using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Data.UnitOfWork.Interfaces;

namespace wolds_hr_api.Data.UnitOfWork;

public class AccountUnitOfWork(IAccountRepository account,
                               WoldsHrDbContext dbContext) : IAccountUnitOfWork
{
    public IAccountRepository Account { get; } = account;

    private readonly WoldsHrDbContext _dbContext = dbContext;

    public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
}