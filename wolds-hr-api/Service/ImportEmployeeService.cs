using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class ImportEmployeeService(IValidator<Employee> validator,
                                   IDepartmentRepository departmentRepository,
                                   IEmployeeUnitOfWork employeeUnitOfWork,
                                   IImportEmployeeHistoryUnitOfWork importEmployeeHistoryUnitOfWork,
                                   ILogger<ImportEmployeeService> logger) : IImportEmployeeService
{
    private readonly IImportEmployeeHistoryUnitOfWork _importEmployeeHistoryUnitOfWork = importEmployeeHistoryUnitOfWork;
    private readonly IEmployeeUnitOfWork _employeeUnitOfWork = employeeUnitOfWork;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IValidator<Employee> _validator = validator;
    private readonly ILogger<ImportEmployeeService> _logger = logger;

    public async Task<ImportEmployeeHistorySummaryResponse> ImportAsync(List<String> fileLines)
    {
        int importedEmployees = 0;
        int importEmployeesExisting = 0;
        int importEmployeesErrors = 0;

        var importEmployeeHistory = await AddImportEmployeeHistoryAsync();

        foreach (var (line, index) in fileLines.Select((val, idx) => (val, idx)))
        {
            try
            {
                if (index == 0) continue;

                var employee = ParseEmployeeFromCsv(line);

                if (await EmployeeExistsAsync(employee, importEmployeeHistory.Id))
                {
                    importEmployeesExisting++;
                    continue;
                }

                if (await ValidateAndHandleAsync(employee, line, importEmployeeHistory.Id))
                {
                    await AddEmployeeAsync(employee, importEmployeeHistory.Id);
                    importedEmployees++;
                }
                else
                {
                    importEmployeesErrors++;
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Import Employee: {line}, Error: {ex.Message}");
                await AddImportEmployeeFailedAsync(line, importEmployeeHistory.Id, ex.Message);
                importEmployeesErrors++;
                continue;
            }
        }

        _logger.LogInformation("Imported {importedEmployees} employees (Success)", importedEmployees);
        _logger.LogInformation("Imported {importEmployeesExisting} employees (Existing)", importEmployeesExisting);
        _logger.LogInformation("Imported {importEmployeesErrors} employees (Failed)", importEmployeesErrors);

        return new ImportEmployeeHistorySummaryResponse()
        {
            Id = importEmployeeHistory.Id,
            Date = DateTime.Now,
            ImportedEmployeesCount = importedEmployees,
            ImportEmployeesExistingCount = importEmployeesExisting,
            ImportEmployeesErrorsCount = importEmployeesErrors
        };
    }

    private async Task<bool> ValidateAndHandleAsync(Employee employee, string rawLine, Guid historyId)
    {
        var result = await _validator.ValidateAsync(employee, opts => opts.IncludeRuleSets("AddUpdate"));

        if (result.IsValid) return true;

        await AddImportEmployeeFailedAsync(rawLine, historyId, result.Errors.Select(e => e.ErrorMessage));
        return false;
    }

    private async Task AddEmployeeAsync(Employee employee, Guid importEmployeeHistoryId)
    {
        employee.ImportEmployeeHistoryId = importEmployeeHistoryId;
        _employeeUnitOfWork.Employee.Add(employee);
        await _employeeUnitOfWork.SaveChangesAsync();

        return;
    }

    public async Task<bool> MaximumNumberOfEmployeesReachedAsync(List<String> fileLines)
    {
        var numberOfEmployeesToImport = fileLines.Count;
        var numberOfEmloyees = await _employeeUnitOfWork.Employee.CountAsync();

        if (numberOfEmployeesToImport + numberOfEmloyees > Constants.MaxNumberOfEmployees)
        {
            return true;
        }

        return false;
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
        var employeeExists = await _employeeUnitOfWork.Employee.ExistsAsync(employee.Surname, employee.FirstName, employee.DateOfBirth);
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

            _importEmployeeHistoryUnitOfWork.ExistingHistory.Add(existingEmployee);
            await _importEmployeeHistoryUnitOfWork.SaveChangesAsync();

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
            Errors = errors.Select(e => new ImportEmployeeFailedErrorHistory
            {
                Error = e
            }).ToList()
        };

        _importEmployeeHistoryUnitOfWork.FailedHistory.Add(importEmployeeFailedHistory);
        await _importEmployeeHistoryUnitOfWork.SaveChangesAsync();
    }

    private async Task AddImportEmployeeFailedAsync(string employeeLine, Guid importEmployeeHistoryId, string error)
    {
        await AddImportEmployeeFailedAsync(employeeLine, importEmployeeHistoryId, [error]);
    }

    private async Task<ImportEmployeeHistory> AddImportEmployeeHistoryAsync()
    {
        ImportEmployeeHistory importEmployeeHistory = new()
        {
            Date = DateTime.Now,
            ImportedEmployees = [],
            ExistingEmployees = [],
            FailedEmployees = []
        };

        _importEmployeeHistoryUnitOfWork.History.Add(importEmployeeHistory);
        await _importEmployeeHistoryUnitOfWork.SaveChangesAsync();

        if (importEmployeeHistory.Id == Guid.Empty)
            throw new ImportEmployeeHistoryNotCreated("Employee import record was not created.");

        return importEmployeeHistory;
    }

    public async Task<List<string>> ReadAllLinesAsync(IFormFile file)
    {
        var lines = new List<string>();
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (!string.IsNullOrWhiteSpace(line))
                lines.Add(line);
        }

        return lines;
    }
}