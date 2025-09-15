using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IImportEmployeeFailedHistoryRepository
{
    Task<ImportEmployeeFailedHistory> AddAsync(ImportEmployeeFailedHistory importEmployeeFailedHistory);
    Task<List<ImportEmployeeFailedHistory>> GetAsync(Guid id, int page, int pageSize);
    Task<int> CountAsync(Guid id);
}
