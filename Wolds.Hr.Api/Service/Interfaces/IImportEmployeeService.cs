using Wolds.Hr.Api.Library.Dto.Responses;

namespace Wolds.Hr.Api.Service.Interfaces;

internal interface IImportEmployeeService
{
    Task<ImportEmployeeHistorySummaryResponse> ImportFromFileAsync(IFormFile file);
    Task<bool> MaximumNumberOfEmployeesReachedAsync(List<String> fileLines);
}