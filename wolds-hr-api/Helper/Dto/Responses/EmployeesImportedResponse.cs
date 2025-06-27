using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper.Dto.Responses;

public class EmployeesImportedResponse(List<Employee> existingEmployees, int employeeImportId, List<string> employeesErrorImporting)
{
    public List<Employee> ExistingEmployees { get; set; } = existingEmployees;
    public int EmployeeImportId { get; set; } = employeeImportId;
    public List<string> EmployeesErrorImporting { get; set; } = employeesErrorImporting;
}