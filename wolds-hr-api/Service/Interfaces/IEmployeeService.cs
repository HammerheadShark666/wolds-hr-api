using employee_test_api.Domain;
using employee_test_api.Helpers.Dto.Responses;

namespace employee_test_api.Services.Interfaces;

public interface IEmployeeService
{
    EmployeePagedResponse Search(string keyword, int page, int pageSize);
    Employee? Get(int id);
    Task<(bool isValid, Employee? Employee, List<string>? Errors)> Add(Employee employee);
    Task<(bool isValid, Employee? Employee, List<string>? Errors)> Update(Employee updatedEmployee);
    int Delete(int id);
}