using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IEmployeeImportService
{
    Task<EmployeeImportResponse> ImportAsync(IFormFile file);
    Task<EmployeePagedResponse> GetImportedEmployeesAsync(int id, int page, int pageSize);
    Task<ExistingEmployeePagedResponse> GetExistingEmployeesImportedAsync(int id, int page, int pageSize);
    Task<bool> MaximumNumberOfEmployeesReachedAsync(IFormFile file);
    Task DeleteAsync(int id);
    Task<List<EmployeeImportResponse>> GetAsync();
}