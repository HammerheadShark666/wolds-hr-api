using Wolds.Hr.Api.Domain;
using Wolds.Hr.Api.Library.Dto.Responses;

namespace Wolds.Hr.Api.Data.Interfaces;

public interface IImportEmployeeHistoryRepository
{
    Task<List<ImportEmployeeHistory>> GetAsync();
    void Add(ImportEmployeeHistory importEmployeeHistory);
    Task DeleteAsync(Guid id);
    Task<List<ImportEmployeeHistoryLatestResponse>> GetLatestAsync(int numberToGet);
}