using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IImportEmployeeHistoryRepository
{
    Task<List<ImportEmployeeHistory>> GetAsync();
    void Add(ImportEmployeeHistory importEmployeeHistory);
    Task DeleteAsync(Guid id);
}