using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper.Dto.Responses;

//public class EmployeeImportResponse(List<Employee> existingEmployees, EmployeePagedResponse importedEmployees, List<string> employeesErrorImporting)
//{
//    public List<Employee> ExistingEmployees { get; set; } = existingEmployees;
//    public EmployeePagedResponse ImportedEmployees { get; set; } = importedEmployees;
//    public List<string> EmployeesErrorImporting { get; set; } = employeesErrorImporting;
//}


public class EmployeeImportResponse(List<Employee> existingEmployees, int employeeImportId, List<string> employeesErrorImporting)
{
    public List<Employee> ExistingEmployees { get; set; } = existingEmployees;
    public int EmployeeImportId { get; set; } = employeeImportId;
    //public EmployeePagedResponse ImportedEmployees { get; set; } = importedEmployees;
    public List<string> EmployeesErrorImporting { get; set; } = employeesErrorImporting;
}