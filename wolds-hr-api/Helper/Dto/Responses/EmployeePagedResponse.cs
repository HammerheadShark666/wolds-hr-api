using wolds_hr_api.Domain;

namespace wolds_hr_api.Helpers.Dto.Responses;

public class EmployeePagedResponse
{
    public List<Employee> Employees { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalEmployees / PageSize);
}
