using Wolds.Hr.Api.Domain;

namespace Wolds.Hr.Api.Data.Interfaces;

public interface IImportEmployeeExistingHistoryRepository
{
    void Add(ImportEmployeeExistingHistory employee);
    Task<(List<ImportEmployeeExistingHistory>, int)> GetAsync(Guid id, int page, int pageSize);
}