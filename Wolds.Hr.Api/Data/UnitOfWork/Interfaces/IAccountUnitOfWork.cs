using Wolds.Hr.Api.Data.Interfaces;

namespace Wolds.Hr.Api.Data.UnitOfWork.Interfaces;

internal interface IAccountUnitOfWork
{
    IAccountRepository Account { get; }
    Task SaveChangesAsync();
}
