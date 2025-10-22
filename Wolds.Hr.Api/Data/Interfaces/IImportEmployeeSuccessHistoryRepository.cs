using Wolds.Hr.Api.Domain;

namespace Wolds.Hr.Api.Data.Interfaces;

public interface IImportEmployeeSuccessHistoryRepository
{
    Task<int> CountAsync(Guid id);
    Task<(List<Employee>, int)> GetAsync(Guid id, int page, int pageSize);
}