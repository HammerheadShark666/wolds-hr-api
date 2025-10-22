using Wolds.Hr.Api.Domain;

namespace Wolds.Hr.Api.Data.Interfaces;

public interface IImportEmployeeFailedHistoryRepository
{
    void Add(ImportEmployeeFailedHistory importEmployeeFailedHistory);
    Task<(List<ImportEmployeeFailedHistory>, int)> GetAsync(Guid id, int page, int pageSize);
}
