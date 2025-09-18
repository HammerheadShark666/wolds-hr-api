using wolds_hr_api.Data.Interfaces;

namespace wolds_hr_api.Data.UnitOfWork.Interfaces;

public interface IEmployeeUnitOfWork
{
    IEmployeeRepository Employee { get; }
    Task SaveChangesAsync();
}
