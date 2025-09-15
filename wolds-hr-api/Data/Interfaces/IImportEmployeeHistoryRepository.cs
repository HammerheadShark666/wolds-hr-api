using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IImportEmployeeHistoryRepository
{
    Task<List<ImportEmployeeHistory>> GetAsync();
    //  Task<List<ImportEmployeeHistoryResponse>> GetImportEmployeeHistoryAsync();
    Task<ImportEmployeeHistory> AddAsync();
    // Task<int> CountAsync(Guid id);
    //Task<List<Employee>> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize);
    //Task<int> CountImportedExistingEmployeesHistoryAsync(Guid id);
    //Task<List<ImportEmployeeExistingHistory>> GetImportedExistingEmployeesHistoryAsync(Guid id, int page, int pageSize);
    Task DeleteAsync(Guid id);
}