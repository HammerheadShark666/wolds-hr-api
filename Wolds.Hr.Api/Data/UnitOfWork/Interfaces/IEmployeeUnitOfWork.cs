using Wolds.Hr.Api.Data.Interfaces;

namespace Wolds.Hr.Api.Data.UnitOfWork.Interfaces;

internal interface IEmployeeUnitOfWork
{
    IEmployeeRepository Employee { get; }
    Task SaveChangesAsync();
}
