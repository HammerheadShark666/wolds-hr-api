using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper.Dto.Responses;

public class EmployeeImportResponse(List<Employee> existingEmployees, List<Employee> employeesImported, List<string> employeesErrorImporting)
{
    public List<Employee> ExistingEmployees { get; set; } = existingEmployees;

    public List<Employee> EmployeesImported { get; set; } = employeesImported;

    public List<string> EmployeesErrorImporting { get; set; } = employeesErrorImporting;
}