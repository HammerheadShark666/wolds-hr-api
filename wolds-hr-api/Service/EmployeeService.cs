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

    public EmployeePagedResponse Search(string keyword, int departmentId, int page, int pageSize)
    {
        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = _employeeRepository.CountEmployees(keyword, departmentId),
            Employees = _employeeRepository.GetEmployees(keyword, departmentId, page, pageSize)
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
        var employee = _employeeRepository.Get(id) ?? throw new EmployeeNotFoundException("Employee not found.");
        string newFileName = FileHelper.getGuidFileName(Constants.FileExtensionJpg);
        string originalFileName = employee.Photo ?? "";

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

    private async Task DeleteOriginalFileAsync(string originalFileName, string newFileName, string container)
    {
        EditPhoto editPhoto = _photoHelper.WasPhotoEdited(originalFileName, newFileName, Constants.DefaultEmployeePhotoFileName);
        if (editPhoto.PhotoWasChanged)
            await _azureStorageHelper.DeleteBlobInAzureStorageContainerAsync(editPhoto.OriginalPhotoName, container);
    }
}