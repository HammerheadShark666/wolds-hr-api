using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helpers.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IEmployeeService
{
    EmployeePagedResponse Search(string keyword, int page, int pageSize);
    Employee? Get(long id);
    Task<(bool isValid, Employee? Employee, List<string>? Errors)> AddAsync(Employee employee);
    Task<(bool isValid, Employee? Employee, List<string>? Errors)> UpdateAsync(Employee updatedEmployee);
    void Delete(long id);
    Task<string> UpdateEmployeePhotoAsync(long id, IFormFile file);
    bool Exists(long id);
    Task<EmployeeImportResponse> ImportAsync(IFormFile file);
    EmployeePagedResponse GetImported(DateOnly importDate, int page, int pageSize);
}