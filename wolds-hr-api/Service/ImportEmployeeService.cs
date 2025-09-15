using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class ImportEmployeeService(IValidator<Employee> validator,
                                   IDepartmentRepository departmentRepository,
                                   IEmployeeRepository employeeRepository,
                                   IImportEmployeeHistoryRepository importEmployeeHistoryRepository,
                                   IImportEmployeeExistingHistoryRepository importEmployeeExistingRepository,
                                   IImportEmployeeFailedHistoryRepository importEmployeeFailedRepository) : IImportEmployeeService
{
    private readonly IImportEmployeeHistoryRepository _importEmployeeHistoryRepository = importEmployeeHistoryRepository;
    private readonly IImportEmployeeExistingHistoryRepository _importEmployeeExistingRepository = importEmployeeExistingRepository;
    private readonly IImportEmployeeFailedHistoryRepository _importEmployeeFailedRepository = importEmployeeFailedRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IValidator<Employee> _validator = validator;

    public async Task<ImportEmployeeHistorySummaryResponse> ImportAsync(IFormFile file)
    {
        int importedEmployees = 0;
        int importEmployeesExisting = 0;
        int importEmployeesErrors = 0;
        var isFirstLine = true;

        var importEmployeeHistory = await _importEmployeeHistoryRepository.AddAsync();
        if (importEmployeeHistory.Id == Guid.Empty)
            throw new ImportEmployeeHistoryNotCreated("Employee import record was not created.");

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var employeeLine = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(employeeLine)) continue;

            try
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }

                var employee = ParseEmployeeFromCsv(employeeLine);

                if (await EmployeeExistsAsync(employee, importEmployeeHistory.Id))
                {
                    importEmployeesExisting++;
                    continue;
                }

                var result = await _validator.ValidateAsync(employee);

                if (result.IsValid)
                {
                    await AddEmployeeAsync(employee, importEmployeeHistory.Id);
                    importedEmployees++;
                }
                else
                {
                    await AddImportEmployeeFailedAsync(employeeLine, importEmployeeHistory.Id, result);
                    importEmployeesErrors++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import Employee: {employeeLine}, Error: {ex.Message}");
                await AddImportEmployeeFailedAsync(employeeLine, importEmployeeHistory.Id, ex.Message);
                importEmployeesErrors++;
                continue;
            }
        }

        return new ImportEmployeeHistorySummaryResponse()
        {
            Id = importEmployeeHistory.Id,
            Date = DateTime.Now,
            ImportedEmployeesCount = importedEmployees,
            ImportEmployeesExistingCount = importEmployeesExisting,
            ImportEmployeesErrorsCount = importEmployeesErrors
        };
    }

    private async Task AddEmployeeAsync(Employee employee, Guid importEmployeeHistoryId)
    {
        employee.ImportEmployeeHistoryId = importEmployeeHistoryId;
        await _employeeRepository.AddAsync(employee);
        return;
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

    private async Task<bool> EmployeeExistsAsync(Employee employee, Guid importEmployeeHistoryId)
    {
        var employeeExists = await _employeeRepository.ExistsAsync(employee.Surname, employee.FirstName, employee.DateOfBirth);
        if (employeeExists)
        {
            var existingEmployee = new ImportEmployeeExistingHistory()
            {
                Surname = employee.Surname,
                FirstName = employee.FirstName,
                DateOfBirth = employee.DateOfBirth,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                ImportEmployeeHistoryId = importEmployeeHistoryId
            };

            await _importEmployeeExistingRepository.Add(existingEmployee);

            return true;
        }

        return false;
    }

    private async Task AddImportEmployeeFailedAsync(string employeeLine, Guid importEmployeeHistoryId, IEnumerable<string> errors)
    {
        var importEmployeeFailedHistory = new ImportEmployeeFailedHistory
        {
            Employee = employeeLine,
            ImportEmployeeHistoryId = importEmployeeHistoryId,
            Errors = [.. errors.Select(e => new ImportEmployeeFailedErrorHistory {
                        Error = e
                     })]
        };

        await _importEmployeeFailedRepository.AddAsync(importEmployeeFailedHistory);
    }

    private async Task AddImportEmployeeFailedAsync(string employeeLine, Guid importEmployeeHistoryId, FluentValidation.Results.ValidationResult result)
    {
        await AddImportEmployeeFailedAsync(employeeLine, importEmployeeHistoryId, result.Errors.Select(e => e.ErrorMessage));
    }

    private async Task AddImportEmployeeFailedAsync(string employeeLine, Guid importEmployeeHistoryId, string error)
    {
        await AddImportEmployeeFailedAsync(employeeLine, importEmployeeHistoryId, [error]);
    }

}