using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IImportEmployeeFailedHistoryRepository
{
    void Add(ImportEmployeeFailedHistory importEmployeeFailedHistory);
    Task<(List<ImportEmployeeFailedHistory>, int)> GetAsync(Guid id, int page, int pageSize);
}
