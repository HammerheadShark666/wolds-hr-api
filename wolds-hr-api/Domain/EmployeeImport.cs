namespace wolds_hr_api.Domain;

public class EmployeeImport
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public required List<Employee> Employees { get; set; } = [];

    public required List<ExistingEmployee> ExistingEmployees { get; set; } = [];
}