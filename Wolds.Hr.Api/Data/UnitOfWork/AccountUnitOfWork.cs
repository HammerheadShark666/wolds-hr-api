using Wolds.Hr.Api.Data.Context;
using Wolds.Hr.Api.Data.Interfaces;
using Wolds.Hr.Api.Data.UnitOfWork.Interfaces;

namespace Wolds.Hr.Api.Data.UnitOfWork;

internal sealed class AccountUnitOfWork(IAccountRepository account,
                               WoldsHrDbContext dbContext) : IAccountUnitOfWork
{
    public IAccountRepository Account { get; } = account;

    private readonly WoldsHrDbContext _dbContext = dbContext;

    public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
}