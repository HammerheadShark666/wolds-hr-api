using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper.Dto.Responses;

public class EmployeeImportResponse(List<Employee> existingEmployees, EmployeePagedResponse todaysImportedEmployees, List<string> employeesErrorImporting)
{
    public List<Employee> ExistingEmployees { get; set; } = existingEmployees;

    public EmployeePagedResponse TodaysImportedEmployees { get; set; } = todaysImportedEmployees;

    public List<string> EmployeesErrorImporting { get; set; } = employeesErrorImporting;
}