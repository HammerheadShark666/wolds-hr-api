using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IImportEmployeeService
{
    Task<ImportEmployeeHistorySummaryResponse> ImportFromFileAsync(IFormFile file);
    Task<bool> MaximumNumberOfEmployeesReachedAsync(List<String> fileLines);
}