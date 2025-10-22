using Wolds.Hr.Api.Library.Dto.Requests.Employee;
using Wolds.Hr.Api.Library.Dto.Responses;

namespace Wolds.Hr.Api.Service.Interfaces;

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