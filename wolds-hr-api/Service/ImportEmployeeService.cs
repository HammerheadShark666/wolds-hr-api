using FluentValidation;
using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Validation;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class ImportEmployeeService(IValidator<Employee> _validator,
                                   IEmployeeUnitOfWork _employeeUnitOfWork,
                                   IImportEmployeeHistoryUnitOfWork _importEmployeeHistoryUnitOfWork,
                                   ILogger<ImportEmployeeService> _logger) : IImportEmployeeService
{
    public async Task<ImportEmployeeHistorySummaryResponse> ImportFromFileAsync(IFormFile file)
    {
        var fileLines = await FileHelper.ReadAllLinesAsync(file);

        if (await MaximumNumberOfEmployeesReachedAsync(fileLines))
            throw new InvalidOperationException($"Maximum number of employees reached: {Constants.MaxNumberOfEmployees}");

        return await ImportAsync(fileLines);
    }

    private async Task<ImportEmployeeHistorySummaryResponse> ImportAsync(List<String> fileLines)
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

                if (!EmployeeCsvParser.TryParse(line, out var employee, out var error) || employee == null)
                {
                    await AddImportEmployeeFailedAsync(line, importEmployeeHistory.Id, error ?? "Parsing failed");
                    importEmployeesErrors++;
                    continue;
                }

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
                _logger.LogError($"Import Employee: {line}, Error: {ex.Message}");
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
        var result = await EmployeeValidationHelper.ValidateEmployeeAsync(employee, _validator);
        if (result.IsValid) return true;

        await AddImportEmployeeFailedAsync(rawLine, historyId, ValidationErrorFormatter.ExtractErrorMessages(result));

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
        if (fileLines == null || fileLines.Count == 0)
            return false;

        var numberOfEmployeesToImport = fileLines.Count;
        var numberOfEmloyees = await _employeeUnitOfWork.Employee.CountAsync();

        return EmployeeLimitHelper.WillExceedLimit(numberOfEmloyees, numberOfEmployeesToImport, Constants.MaxNumberOfEmployees);
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
}