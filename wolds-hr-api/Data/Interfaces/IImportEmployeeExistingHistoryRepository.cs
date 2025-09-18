using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IImportEmployeeExistingHistoryRepository
{
    ImportEmployeeExistingHistory Add(ImportEmployeeExistingHistory employee);
    Task<int> CountAsync(Guid id);
    Task<List<ImportEmployeeExistingHistory>> GetAsync(Guid id, int page, int pageSize);
}