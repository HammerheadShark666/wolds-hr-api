using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IImportEmployeeSuccessHistoryRepository
{
    Task<int> CountAsync(Guid id);
    Task<(List<Employee>, int)> GetAsync(Guid id, int page, int pageSize);
}