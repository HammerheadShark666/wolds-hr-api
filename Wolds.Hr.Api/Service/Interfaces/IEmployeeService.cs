using wolds_hr_api.Library.Dto.Requests.Employee;
using wolds_hr_api.Library.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

internal interface IEmployeeService
{
    Task<EmployeePagedResponse> SearchAsync(string keyword, Guid? departmentId, int page, int pageSize);
    Task<EmployeeResponse?> GetAsync(Guid id);
    Task<(bool isValid, EmployeeResponse? Employee, List<string>? Errors)> AddAsync(AddEmployeeRequest addEmployeeRequest);
    Task<(bool isValid, EmployeeResponse? Employee, List<string>? Errors)> UpdateAsync(UpdateEmployeeRequest updateEmployeeRequest);
    Task DeleteAsync(Guid id);
    Task<string> UpdateEmployeePhotoAsync(Guid id, IFormFile file);
    Task<bool> ExistsAsync(Guid id);
}