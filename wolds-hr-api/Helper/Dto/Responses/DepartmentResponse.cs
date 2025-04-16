using employee_test_api.Domain;

namespace employee_test_api.Helpers.Dto.Responses;

public class DepartmentResponse
{
    public List<Department> Departments { get; set; } = [];
}