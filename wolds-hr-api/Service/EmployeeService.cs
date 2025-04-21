using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Helpers.Dto.Responses;
using wolds_hr_api.Service.Interfaces;
using static wolds_hr_api.Helper.PhotoHelper;

namespace wolds_hr_api.Service;

public class EmployeeService : IEmployeeService
{
    private readonly IAzureStorageBlobHelper _azureStorageHelper;
    private readonly IPhotoHelper _photoHelper;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IValidator<Employee> _validator;

    public EmployeeService(IValidator<Employee> validator,
                           IEmployeeRepository employeeRepository,
                           IAzureStorageBlobHelper azureStorageHelper,
                           IPhotoHelper photoHelper)
    {
        _validator = validator;
        _azureStorageHelper = azureStorageHelper;
        _photoHelper = photoHelper;
        _employeeRepository = employeeRepository;
    }

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

    public Employee? Get(long id)
    {
        return _employeeRepository.Get(id);
    }

    public async Task<(bool isValid, Employee? Employee, List<string>? Errors)> Add(Employee employee)
    {
        var result = await _validator.ValidateAsync(employee, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        if (!result.IsValid)
        {
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());
        }

        _employeeRepository.Add(employee);
        return (true, employee, null);
    }

    public async Task<(bool isValid, Employee? Employee, List<string>? Errors)> Update(Employee employee)
    {
        var result = await _validator.ValidateAsync(employee, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        if (!result.IsValid)
        {
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());
        }

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

    private async Task DeleteOriginalFileAsync(string originalFileName, string newFileName, string container)
    {
        EditPhoto editPhoto = _photoHelper.WasPhotoEdited(originalFileName, newFileName, Constants.DefaultEmployeePhotoFileName);
        if (editPhoto.PhotoWasChanged)
        {
            await _azureStorageHelper.DeleteBlobInAzureStorageContainerAsync(editPhoto.OriginalPhotoName, container);
        }
    }

    public bool Exists(long id)
    {
        return _employeeRepository.Exists(id);
    }
}