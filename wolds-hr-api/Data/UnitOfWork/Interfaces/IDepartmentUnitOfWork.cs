using wolds_hr_api.Data.Interfaces;

namespace wolds_hr_api.Data.UnitOfWork.Interfaces;

public interface IDepartmentUnitOfWork
{
    IDepartmentRepository Department { get; }
    Task SaveChangesAsync();
}
