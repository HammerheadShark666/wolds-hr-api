using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IEmployeeService
{
    Task<EmployeePagedResponse> SearchAsync(string keyword, Guid? departmentId, int page, int pageSize);
    Task<Employee?> GetAsync(Guid id);
    Task<(bool isValid, Employee? Employee, List<string>? Errors)> AddAsync(Employee employee);
    Task<(bool isValid, Employee? Employee, List<string>? Errors)> UpdateAsync(Employee updatedEmployee);
    Task DeleteAsync(Guid id);
    Task<string> UpdateEmployeePhotoAsync(Guid id, IFormFile file);
    Task<bool> ExistsAsync(Guid id);
}