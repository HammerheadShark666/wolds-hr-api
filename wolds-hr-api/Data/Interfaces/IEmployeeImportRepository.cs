using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IEmployeeImportRepository
{
    List<EmployeeImport> Get();
    EmployeeImport Add();
    int CountImportedEmployees(int id);
    List<Employee> GetImportedEmployees(int id, int page, int pageSize);
}