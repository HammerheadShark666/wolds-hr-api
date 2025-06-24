using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class EmployeeImportService(IDepartmentRepository departmentRepository, IEmployeeImportRepository employeeImportRepository, IEmployeeRepository employeeRepository) : IEmployeeImportService
{
    private readonly IEmployeeImportRepository _employeeImportRepository = employeeImportRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;

    public async Task<EmployeeImportResponse> ImportAsync(IFormFile file)
    {
        List<Employee> ExistingEmployees = [];
        List<Employee> EmployeesImported = [];
        List<string> EmployeesErrorImporting = [];
        EmployeeImport employeeImport = new();

        bool createEmployeeImportRecord = true;

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {

            if (createEmployeeImportRecord)
            {
                employeeImport = _employeeImportRepository.Add();
                createEmployeeImportRecord = false;
            }

            var employeeLine = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(employeeLine)) continue;

            try
            {
                var employee = ParseEmployeeFromCsv(employeeLine);

                if (employee.Surname == "Surname")
                    continue;

                var (employeeExists, existingEmployees) = EmployeeExists(employee, ExistingEmployees);

                ExistingEmployees = existingEmployees;
                if (employeeExists)
                    continue;

                employee.EmployeeImportId = employeeImport.Id;

                _employeeRepository.Add(employee);

                if (employee.DepartmentId != null)
                    employee.Department = _departmentRepository.Get(employee.DepartmentId);

                EmployeesImported.Add(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import Employee: {employeeLine}, Error: {ex.Message}");
                EmployeesErrorImporting.Add(employeeLine);
                continue;
            }
        }

        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = 1,
            PageSize = 10,
            TotalEmployees = EmployeesImported.Count,
            Employees = [.. EmployeesImported.OrderBy(e => e.Surname)]
        };

        return new EmployeeImportResponse(ExistingEmployees, employeePagedResponse, EmployeesErrorImporting);
    }

    public EmployeePagedResponse GetImported(int id, int page, int pageSize)
    {
        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = _employeeRepository.CountImportedEmployees(id),
            Employees = _employeeRepository.GetImportedEmployees(id, page, pageSize)
        };

        return employeePagedResponse;
    }

    public bool MaximumNumberOfEmployeesReached(IFormFile file)
    {
        var numberOfEmployeesToImport = NumberOfEmployeesToImport(file);
        var numberOfEmloyees = _employeeRepository.Count();

        if (numberOfEmployeesToImport + numberOfEmloyees > Constants.MaxNumberOfEmployees)
        {
            return true;
        }

        return false;
    }

    private static int NumberOfEmployeesToImport(IFormFile file)
    {
        int numberOfEmployees = 0;

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var employeeLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(employeeLine)) continue;

            var employee = ParseEmployeeFromCsv(employeeLine);
            if (employee.Surname == "Surname") continue;

            var values = employeeLine.Split(',');
            if (values.Length > 1)
                numberOfEmployees++;
        }

        return numberOfEmployees;
    }

    private static Employee ParseEmployeeFromCsv(string employeeLine)
    {
        var values = employeeLine.Split(',');

        return new Employee
        {
            Surname = values[1],
            FirstName = values[2],
            DateOfBirth = DateOnly.TryParse(values[3], out var dob) ? dob : null,
            HireDate = DateOnly.TryParse(values[4], out var hireDate) ? hireDate : null,
            DepartmentId = int.TryParse(values[5], out var deptId) ? deptId : null,
            Email = values[6],
            PhoneNumber = values[7],
            Created = DateOnly.FromDateTime(DateTime.Now)
        };
    }

    private (bool employeeExists, List<Employee> existingEmployees) EmployeeExists(Employee employee, List<Employee> existingEmployees)
    {
        var employeeExists = _employeeRepository.Exists(employee.Surname, employee.FirstName, employee.DateOfBirth);
        if (employeeExists)
        {
            existingEmployees.Add(employee);
        }

        return (employeeExists, existingEmployees);
    }
}