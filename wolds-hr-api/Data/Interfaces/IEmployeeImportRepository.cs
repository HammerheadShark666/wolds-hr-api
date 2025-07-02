using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IEmployeeImportRepository
{
    Task<List<EmployeeImport>> GetAsync();
    Task<EmployeeImport> AddAsync();
    Task<int> CountImportedEmployeesAsync(int id);
    Task<List<Employee>> GetImportedEmployeesAsync(int id, int page, int pageSize);
    Task<int> CountImportedExistingEmployeesAsync(int id);
    Task<List<ExistingEmployee>> GetImportedExistingEmployeesAsync(int id, int page, int pageSize);
    Task DeleteAsync(int id);
}