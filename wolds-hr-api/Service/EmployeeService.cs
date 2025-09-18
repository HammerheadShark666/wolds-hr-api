using FluentValidation;
using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Requests.Employee;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Helper.Mappers;
using wolds_hr_api.Service.Interfaces;
using static wolds_hr_api.Helper.PhotoHelper;

namespace wolds_hr_api.Service;

public class EmployeeService(IValidator<Employee> validator,
                             IEmployeeUnitOfWork employeeUnitOfWork,
                             IAzureStorageBlobHelper azureStorageHelper,
                             IPhotoHelper photoHelper) : IEmployeeService
{
    private readonly IAzureStorageBlobHelper _azureStorageHelper = azureStorageHelper;
    private readonly IPhotoHelper _photoHelper = photoHelper;
    private readonly IEmployeeUnitOfWork _employeeUnitOfWork = employeeUnitOfWork;
    private readonly IValidator<Employee> _validator = validator;

    public async Task<EmployeePagedResponse> SearchAsync(string keyword, Guid? departmentId, int page, int pageSize)
    {
        var countTask = _employeeUnitOfWork.Employee.CountAsync(keyword, departmentId);
        var employeesTask = _employeeUnitOfWork.Employee.GetAsync(keyword, departmentId, page, pageSize);

        await Task.WhenAll(countTask, employeesTask);

        return new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = countTask.Result,
            Employees = EmployeeMapper.ToEmployeesResponse(employeesTask.Result)
        };
    }

    public async Task<EmployeeResponse?> GetAsync(Guid id)
    {
        var employee = await _employeeUnitOfWork.Employee.GetAsync(id);
        return employee == null ? null : EmployeeMapper.ToEmployeeResponse(employee);
    }

    public async Task<(bool isValid, EmployeeResponse? Employee, List<string>? Errors)> AddAsync(AddEmployeeRequest addEmployeeRequest)
    {
        var employee = EmployeeMapper.ToEmployee(addEmployeeRequest);

        var result = await _validator.ValidateAsync(employee, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        if (!result.IsValid)
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());

        _employeeUnitOfWork.Employee.Add(employee);
        await _employeeUnitOfWork.SaveChangesAsync();

        var newEmployee = await _employeeUnitOfWork.Employee.GetAsync(employee.Id)
          ?? throw new EmployeeNotFoundException("Employee not found after adding.");

        return (true, EmployeeMapper.ToEmployeeResponse(newEmployee), null);
    }

    public async Task<(bool isValid, EmployeeResponse? Employee, List<string>? Errors)> UpdateAsync(UpdateEmployeeRequest updateEmployeeRequest)
    {
        var employee = EmployeeMapper.ToEmployee(updateEmployeeRequest);

        var result = await _validator.ValidateAsync(employee, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        if (!result.IsValid)
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());

        await _employeeUnitOfWork.Employee.UpdateAsync(employee);
        await _employeeUnitOfWork.SaveChangesAsync();

        var updatedEmployee = await _employeeUnitOfWork.Employee.GetAsync(employee.Id)
            ?? throw new EmployeeNotFoundException("Employee not found after updating.");

        return (true, EmployeeMapper.ToEmployeeResponse(updatedEmployee), null);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _employeeUnitOfWork.Employee.DeleteAsync(id);
        await _employeeUnitOfWork.SaveChangesAsync();
        return;
    }

    public async Task<string> UpdateEmployeePhotoAsync(Guid id, IFormFile file)
    {
        var employee = await _employeeUnitOfWork.Employee.GetAsync(id) ?? throw new EmployeeNotFoundException("Employee not found.");
        string newFileName = FileHelper.getGuidFileName(Constants.FileExtensionJpg);
        string originalFileName = employee.Photo ?? "";

        await _azureStorageHelper.SaveBlobToAzureStorageContainerAsync(file, Constants.AzureStorageContainerEmployees, newFileName);

        employee.Photo = newFileName;

        await _employeeUnitOfWork.Employee.UpdateAsync(employee);
        await _employeeUnitOfWork.SaveChangesAsync();

        if (!String.IsNullOrEmpty(originalFileName))
            await DeleteOriginalFileAsync(originalFileName, newFileName, Constants.AzureStorageContainerEmployees);

        return newFileName;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _employeeUnitOfWork.Employee.ExistsAsync(id);
    }

    private async Task DeleteOriginalFileAsync(string originalFileName, string newFileName, string container)
    {
        EditPhoto editPhoto = _photoHelper.WasPhotoEdited(originalFileName, newFileName, Constants.DefaultEmployeePhotoFileName);
        if (editPhoto.PhotoWasChanged)
            await _azureStorageHelper.DeleteBlobInAzureStorageContainerAsync(editPhoto.OriginalPhotoName, container);
    }
}