namespace wolds_hr_api.Helper.Dto.Requests.Employee;

public class EmployeeRequestBase
{
    public string Surname { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? HireDate { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public Guid? DepartmentId { get; set; }
}
