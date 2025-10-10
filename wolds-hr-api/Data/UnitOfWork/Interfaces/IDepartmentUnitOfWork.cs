using wolds_hr_api.Data.Interfaces;

namespace wolds_hr_api.Data.UnitOfWork.Interfaces;

internal interface IDepartmentUnitOfWork
{
    IDepartmentRepository Department { get; }
    Task SaveChangesAsync();
}
