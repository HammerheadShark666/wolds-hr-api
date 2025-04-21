namespace wolds_hr_api.Domain;

public class Employee()
{
    public long Id { get; set; }
    public string Surname { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? HireDate { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public string? Photo { get; set; } = string.Empty;

    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
}