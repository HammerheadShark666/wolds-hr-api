using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IImportEmployeeFailHistoryRepository
{
    Task<ImportEmployeeFailHistory> AddAsync(string employee, Guid importEmployeeHistoryId, string[] errors);
    Task<int> CountImportedFailedEmployeesHistoryAsync(Guid id);
    Task<List<ImportEmployeeFailHistory>> GetImportedFailedEmployeesHistoryAsync(Guid id, int page, int pageSize);
}