using Wolds.Hr.Api.Data.Interfaces;

namespace Wolds.Hr.Api.Data.UnitOfWork.Interfaces;

internal interface IDepartmentUnitOfWork
{
    IDepartmentRepository Department { get; }
    Task SaveChangesAsync();
}
