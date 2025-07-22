using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class EmployeeImportService(IDepartmentRepository departmentRepository, IEmployeeImportRepository employeeImportRepository, IEmployeeRepository employeeRepository, IExistingEmployeeRepository existingEmployeeRepository) : IEmployeeImportService
{
    private readonly IEmployeeImportRepository _employeeImportRepository = employeeImportRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IExistingEmployeeRepository _existingEmployeeRepository = existingEmployeeRepository;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;

    public async Task<EmployeeImportResponse> ImportAsync(IFormFile file)
    {
        List<Employee> EmployeesImported = [];
        List<string> EmployeesErrorImporting = [];
        EmployeeImport employeeImport = new()
        {
            Employees = [],
            ExistingEmployees = []
        };

        bool createEmployeeImportRecord = true;

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var employeeLine = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(employeeLine)) continue;

            try
            {
                if (createEmployeeImportRecord)
                {
                    employeeImport = await _employeeImportRepository.AddAsync();
                    createEmployeeImportRecord = false;
                }

                if (employeeImport.Id == Guid.Empty)
                    throw new EmployeeImportNotCreated("Employee import record was not created.");

                var employee = ParseEmployeeFromCsv(employeeLine);

                if (employee.Surname == "Surname")
                    continue;

                if (await EmployeeExistsAsync(employee, employeeImport.Id))
                    continue;

                employee = await AddEmployeeAsync(employee, employeeImport.Id);

                EmployeesImported.Add(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import Employee: {employeeLine}, Error: {ex.Message}");
                EmployeesErrorImporting.Add(employeeLine);
                continue;
            }
        }

        return new EmployeeImportResponse() { Id = employeeImport.Id, Date = DateTime.Now };
    }


    private async Task<Employee> AddEmployeeAsync(Employee employee, Guid employeeImportId)
    {

        //TODO check to see if department exists is not then throw error


        employee.EmployeeImportId = employeeImportId;

        await _employeeRepository.AddAsync(employee);

        if (employee.DepartmentId != null)
            employee.Department = _departmentRepository.Get(employee.DepartmentId);

        return employee;
    }

    public async Task<EmployeePagedResponse> GetImportedEmployeesAsync(Guid id, int page, int pageSize)
    {
        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _employeeImportRepository.CountImportedEmployeesAsync(id),
            Employees = await _employeeImportRepository.GetImportedEmployeesAsync(id, page, pageSize)
        };

        return employeePagedResponse;
    }

    public async Task<ExistingEmployeePagedResponse> GetExistingEmployeesImportedAsync(Guid id, int page, int pageSize)
    {
        var existingEmployeePagedResponse = new ExistingEmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _employeeImportRepository.CountImportedExistingEmployeesAsync(id),
            ExistingEmployees = await _employeeImportRepository.GetImportedExistingEmployeesAsync(id, page, pageSize)
        };

        return existingEmployeePagedResponse;
    }

    public async Task<bool> MaximumNumberOfEmployeesReachedAsync(IFormFile file)
    {
        var numberOfEmployeesToImport = NumberOfEmployeesToImport(file);
        var numberOfEmloyees = await _employeeRepository.CountAsync();

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
            DepartmentId = Guid.TryParse(values[5], out var deptId) ? deptId : null,
            Email = values[6],
            PhoneNumber = values[7],
            Created = DateOnly.FromDateTime(DateTime.Now)
        };
    }

    private async Task<bool> EmployeeExistsAsync(Employee employee, Guid employeeImportId)
    {
        var employeeExists = await _employeeRepository.ExistsAsync(employee.Surname, employee.FirstName, employee.DateOfBirth);
        if (employeeExists)
        {
            var existingEmployee = new ExistingEmployee()
            {
                Surname = employee.Surname,
                FirstName = employee.FirstName,
                DateOfBirth = employee.DateOfBirth,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                EmployeeImportId = employeeImportId
            };

            _existingEmployeeRepository.Add(existingEmployee);

            return true;
        }

        return false;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _employeeImportRepository.DeleteAsync(id);
        return;
    }

    public async Task<List<EmployeeImportResponse>> GetAsync()
    {
        var employeeImports = await _employeeImportRepository.GetAsync();

        return employeeImports.Select(ei => new EmployeeImportResponse
        {
            Id = ei.Id,
            Date = ei.Date
        }).ToList();
    }
}