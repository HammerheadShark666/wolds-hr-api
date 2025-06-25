using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IEmployeeImportService
{
    Task<EmployeeImportResponse> ImportAsync(IFormFile file);
    EmployeePagedResponse GetImported(int id, int page, int pageSize);
    bool MaximumNumberOfEmployeesReached(IFormFile file);
    void Delete(int id);
}