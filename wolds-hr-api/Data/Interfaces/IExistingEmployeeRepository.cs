using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IExistingEmployeeRepository
{
    ExistingEmployee Add(ExistingEmployee existingEmployee);
}