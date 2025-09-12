using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IImportEmployeeHistoryRepository
{
    Task<List<ImportEmployeeHistory>> GetAsync();
    Task<ImportEmployeeHistory> AddAsync();
    Task<int> CountImportedEmployeesHistoryAsync(Guid id);
    Task<List<Employee>> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize);
    Task DeleteAsync(Guid id);
}