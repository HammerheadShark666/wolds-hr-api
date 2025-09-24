using FluentValidation;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Dto.Requests.Employee;
using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Helper.Mappers;

public class EmployeeMapper
{
    public static Employee ToEmployee(AddEmployeeRequest addEmployeeRequest)
    {
        return _ = new Employee()
        {
            Surname = addEmployeeRequest.Surname,
            FirstName = addEmployeeRequest.FirstName,
            DateOfBirth = addEmployeeRequest.DateOfBirth,
            HireDate = addEmployeeRequest.HireDate,
            Email = addEmployeeRequest.Email,
            PhoneNumber = addEmployeeRequest.PhoneNumber,
            DepartmentId = addEmployeeRequest.DepartmentId
        };
    }

    public static Employee ToEmployee(UpdateEmployeeRequest updateEmployeeRequest)
    {
        return _ = new Employee()
        {
            Id = updateEmployeeRequest.Id,
            Surname = updateEmployeeRequest.Surname,
            FirstName = updateEmployeeRequest.FirstName,
            DateOfBirth = updateEmployeeRequest.DateOfBirth,
            HireDate = updateEmployeeRequest.HireDate,
            Email = updateEmployeeRequest.Email,
            PhoneNumber = updateEmployeeRequest.PhoneNumber,
            DepartmentId = updateEmployeeRequest.DepartmentId
        };
    }

    public static EmployeeResponse ToEmployeeResponse(Employee employee)
    {
        return _ = new EmployeeResponse()
        {
            Id = employee.Id,
            Surname = employee.Surname,
            FirstName = employee.FirstName,
            DateOfBirth = employee.DateOfBirth,
            HireDate = employee.HireDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            DepartmentId = employee.DepartmentId,
            Department = employee.Department,
            Created = employee.Created
        };
    }

    public static EmployeeResponse ToEmployeeResponse(ImportEmployeeExistingHistory employee)
    {
        return _ = new EmployeeResponse()
        {
            Id = employee.Id,
            Surname = employee.Surname,
            FirstName = employee.FirstName,
            DateOfBirth = employee.DateOfBirth,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Created = employee.Created
        };
    }

    public static List<EmployeeResponse> ToEmployeesResponse(List<Employee> employees)
    {
        return [.. employees!.Where(e => e is not null).Select(e => ToEmployeeResponse(e!))];
    }


    public static List<EmployeeResponse> ToEmployeesResponse(List<ImportEmployeeExistingHistory> employees)
    {
        return [.. employees!.Where(e => e is not null).Select(e => ToEmployeeResponse(e!))];
    }

    public static ImportEmployeeFailedResponse ToImportEmployeeFailedResponse(ImportEmployeeFailedHistory failedEmployee)
    {
        return _ = new ImportEmployeeFailedResponse()
        {
            Id = failedEmployee.Id,
            Employee = failedEmployee.Employee,
            Errors = [.. failedEmployee.Errors.Select(e => e.Error.ToString())]
        };
    }

    public static List<ImportEmployeeFailedResponse> ToImportEmployeesFailedResponse(List<ImportEmployeeFailedHistory> failedEmployees)
    {
        return [.. failedEmployees!.Where(e => e is not null).Select(e => ToImportEmployeeFailedResponse(e!))];
    }
}