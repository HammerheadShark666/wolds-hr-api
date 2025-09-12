using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IImportEmployeeService
{
    Task<ImportEmployeeHistoryResponse> ImportAsync(IFormFile file);
    Task<bool> MaximumNumberOfEmployeesReachedAsync(IFormFile file);
}