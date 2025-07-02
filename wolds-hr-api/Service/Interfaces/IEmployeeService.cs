using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IEmployeeService
{
    Task<EmployeePagedResponse> SearchAsync(string keyword, int departmentId, int page, int pageSize);
    Task<Employee?> GetAsync(long id);
    Task<(bool isValid, Employee? Employee, List<string>? Errors)> AddAsync(Employee employee);
    Task<(bool isValid, Employee? Employee, List<string>? Errors)> UpdateAsync(Employee updatedEmployee);
    Task DeleteAsync(long id);
    Task<string> UpdateEmployeePhotoAsync(long id, IFormFile file);
    Task<bool> ExistsAsync(long id);
}