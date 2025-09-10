using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Service.Interfaces;
using static wolds_hr_api.Helper.PhotoHelper;

namespace wolds_hr_api.Service;

public class EmployeeService(IValidator<Employee> validator,
                             IEmployeeRepository employeeRepository,
                             IAzureStorageBlobHelper azureStorageHelper,
                             IPhotoHelper photoHelper) : IEmployeeService
{
    private readonly IAzureStorageBlobHelper _azureStorageHelper = azureStorageHelper;
    private readonly IPhotoHelper _photoHelper = photoHelper;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IValidator<Employee> _validator = validator;

    public async Task<EmployeePagedResponse> SearchAsync(string keyword, Guid? departmentId, int page, int pageSize)
    {
        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _employeeRepository.CountEmployeesAsync(keyword, departmentId),
            Employees = await _employeeRepository.GetEmployeesAsync(keyword, departmentId, page, pageSize)
        };

        return employeePagedResponse;
    }

    public async Task<Employee?> GetAsync(Guid id)
    {
        return await _employeeRepository.GetAsync(id);
    }

    public async Task<(bool isValid, Employee? Employee, List<string>? Errors)> AddAsync(Employee employee)
    {
        var result = await _validator.ValidateAsync(employee, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        if (!result.IsValid)
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());

        var newEmployee = await _employeeRepository.AddAsync(employee) ?? throw new EmployeeNotFoundException("Employee not found after attempt to create");
        newEmployee = await _employeeRepository.GetAsync(newEmployee.Id);

        return (true, newEmployee, null);
    }

    public async Task<(bool isValid, Employee? Employee, List<string>? Errors)> UpdateAsync(Employee employee)
    {
        var result = await _validator.ValidateAsync(employee, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        if (!result.IsValid)
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());

        await _employeeRepository.UpdateAsync(employee);

        return (true, await GetAsync(employee.Id), null);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _employeeRepository.DeleteAsync(id);
        return;
    }

    public async Task<string> UpdateEmployeePhotoAsync(Guid id, IFormFile file)
    {
        var employee = await _employeeRepository.GetAsync(id) ?? throw new EmployeeNotFoundException("Employee not found.");
        string newFileName = FileHelper.getGuidFileName(Constants.FileExtensionJpg);
        string originalFileName = employee.Photo ?? "";

        await _azureStorageHelper.SaveBlobToAzureStorageContainerAsync(file, Constants.AzureStorageContainerEmployees, newFileName);

        employee.Photo = newFileName;
        await _employeeRepository.UpdateAsync(employee);

        if (!String.IsNullOrEmpty(originalFileName))
            await DeleteOriginalFileAsync(originalFileName, newFileName, Constants.AzureStorageContainerEmployees);

        return newFileName;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _employeeRepository.ExistsAsync(id);
    }

    private async Task DeleteOriginalFileAsync(string originalFileName, string newFileName, string container)
    {
        EditPhoto editPhoto = _photoHelper.WasPhotoEdited(originalFileName, newFileName, Constants.DefaultEmployeePhotoFileName);
        if (editPhoto.PhotoWasChanged)
            await _azureStorageHelper.DeleteBlobInAzureStorageContainerAsync(editPhoto.OriginalPhotoName, container);
    }
}