using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IEmployeeImportRepository
{
    Task<List<EmployeeImport>> GetAsync();
    Task<EmployeeImport> AddAsync();
    Task<int> CountImportedEmployeesAsync(Guid id);
    Task<List<Employee>> GetImportedEmployeesAsync(Guid id, int page, int pageSize);
    Task<int> CountImportedExistingEmployeesAsync(Guid id);
    Task<List<ExistingEmployee>> GetImportedExistingEmployeesAsync(Guid id, int page, int pageSize);
    Task DeleteAsync(Guid Guid);
}