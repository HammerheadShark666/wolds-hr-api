using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IEmployeeImportRepository
{
    List<EmployeeImport> Get();
    EmployeeImport Add();
}