using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Helpers.Dto.Responses;
using wolds_hr_api.Service.Interfaces;
using static wolds_hr_api.Helper.PhotoHelper;

namespace wolds_hr_api.Service;

public class EmployeeService(IValidator<Employee> validator,
                             IEmployeeRepository employeeRepository,
                             IDepartmentRepository departmentRepository,
                             IAzureStorageBlobHelper azureStorageHelper,
                             IPhotoHelper photoHelper) : IEmployeeService
{
    private readonly IAzureStorageBlobHelper _azureStorageHelper = azureStorageHelper;
    private readonly IPhotoHelper _photoHelper = photoHelper;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IValidator<Employee> _validator = validator;

    public EmployeePagedResponse Search(string keyword, int page, int pageSize)
    {
        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = _employeeRepository.CountEmployees(keyword),
            Employees = _employeeRepository.GetEmployees(keyword, page, pageSize)
        };

        return employeePagedResponse;
    }

    public EmployeePagedResponse GetImported(DateOnly importDate, int page, int pageSize)
    {
        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = _employeeRepository.CountImportedEmployees(importDate),
            Employees = _employeeRepository.GetImportedEmployees(importDate, page, pageSize)
        };

        return employeePagedResponse;
    }

    public Employee? Get(long id)
    {
        return _employeeRepository.Get(id);
    }

    public async Task<(bool isValid, Employee? Employee, List<string>? Errors)> AddAsync(Employee employee)
    {
        var result = await _validator.ValidateAsync(employee, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        if (!result.IsValid)
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());

        _employeeRepository.Add(employee);
        return (true, employee, null);
    }

    public async Task<(bool isValid, Employee? Employee, List<string>? Errors)> UpdateAsync(Employee employee)
    {
        var result = await _validator.ValidateAsync(employee, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        if (!result.IsValid)
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());

        _employeeRepository.Update(employee);

        return (true, Get(employee.Id), null);
    }

    public void Delete(long id)
    {
        _employeeRepository.Delete(id);
        return;
    }

    public async Task<string> UpdateEmployeePhotoAsync(long id, IFormFile file)
    {
        var employee = _employeeRepository.Get(id);

        //TODO: Sort this 
        if (employee?.Photo == null)
            throw new Exception("");

        string newFileName = FileHelper.getGuidFileName(Constants.FileExtensionJpg);
        string originalFileName = employee.Photo;

        await _azureStorageHelper.SaveBlobToAzureStorageContainerAsync(file, Constants.AzureStorageContainerEmployees, newFileName);

        employee.Photo = newFileName;
        _employeeRepository.Update(employee);

        if (!String.IsNullOrEmpty(originalFileName))
            await DeleteOriginalFileAsync(originalFileName, newFileName, Constants.AzureStorageContainerEmployees);

        return newFileName;
    }

    public bool Exists(long id)
    {
        return _employeeRepository.Exists(id);
    }

    public async Task<EmployeeImportResponse> ImportAsync(IFormFile file)
    {
        List<Employee> ExistingEmployees = [];
        List<Employee> EmployeesImported = [];
        List<string> EmployeesErrorImporting = [];

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var employeeLine = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(employeeLine)) continue;

            try
            {
                var employee = ParseEmployeeFromCsv(employeeLine);

                var (employeeExists, existingEmployees) = EmployeeExists(employee, ExistingEmployees);

                ExistingEmployees = existingEmployees;
                if (employeeExists)
                    continue;

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

        EmployeePagedResponse todaysImportedEmployees = GetImported(DateOnly.FromDateTime(DateTime.Now), 1, 5);

        return new EmployeeImportResponse(ExistingEmployees, todaysImportedEmployees, EmployeesErrorImporting);
    }

    private (bool employeeExists, List<Employee> existingEmployees) EmployeeExists(Employee employee, List<Employee> existingEmployees)
    {
        var employeeExists = _employeeRepository.Exists(employee.Surname, employee.FirstName, employee.DateOfBirth);
        if (employeeExists)
            existingEmployees.Add(employee);

        return (employeeExists, existingEmployees);
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
            WasImported = true
        };
    }

    private async Task DeleteOriginalFileAsync(string originalFileName, string newFileName, string container)
    {
        EditPhoto editPhoto = _photoHelper.WasPhotoEdited(originalFileName, newFileName, Constants.DefaultEmployeePhotoFileName);
        if (editPhoto.PhotoWasChanged)
            await _azureStorageHelper.DeleteBlobInAzureStorageContainerAsync(editPhoto.OriginalPhotoName, container);
    }
}