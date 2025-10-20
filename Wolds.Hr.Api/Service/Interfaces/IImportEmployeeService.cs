using wolds_hr_api.Library.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

internal interface IImportEmployeeService
{
    Task<ImportEmployeeHistorySummaryResponse> ImportFromFileAsync(IFormFile file);
    Task<bool> MaximumNumberOfEmployeesReachedAsync(List<String> fileLines);
}