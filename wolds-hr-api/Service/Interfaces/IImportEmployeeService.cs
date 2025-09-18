using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IImportEmployeeService
{
    Task<ImportEmployeeHistorySummaryResponse> ImportAsync(List<String> fileLines);
    Task<bool> MaximumNumberOfEmployeesReachedAsync(List<String> fileLines);
    Task<List<string>> ReadAllLinesAsync(IFormFile file);
}