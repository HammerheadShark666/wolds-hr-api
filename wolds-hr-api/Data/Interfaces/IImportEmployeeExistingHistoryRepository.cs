using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IImportEmployeeExistingHistoryRepository
{
    void Add(ImportEmployeeExistingHistory employee);
    Task<(List<ImportEmployeeExistingHistory>, int)> GetAsync(Guid id, int page, int pageSize);
}