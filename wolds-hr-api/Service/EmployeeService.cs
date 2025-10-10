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

internal sealed class EmployeeService(IValidator<Employee> _validator,
                                      IEmployeeUnitOfWork _employeeUnitOfWork,
                                      IAzureStorageBlobHelper _azureStorageHelper,
                                      IPhotoHelper _photoHelper) : IEmployeeService
{
    public async Task<EmployeePagedResponse> SearchAsync(string keyword, Guid? departmentId, int page, int pageSize)
    {
        var (employees, totalEmployees) = await _employeeUnitOfWork.Employee.GetAsync(keyword, departmentId, page, pageSize);

        return new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = totalEmployees,
            Employees = EmployeeMapper.ToEmployeesResponse(employees)
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

        await ValidateEmployeeAsync(employee);
        _employeeUnitOfWork.Employee.Add(employee);
        await _employeeUnitOfWork.SaveChangesAsync();

        var newEmployee = await _employeeUnitOfWork.Employee.GetAsync(employee.Id)
          ?? throw new EmployeeNotFoundException("Employee not found after adding.");

        return (true, EmployeeMapper.ToEmployeeResponse(newEmployee), null);
    }

    public async Task<(bool isValid, EmployeeResponse? Employee, List<string>? Errors)> UpdateAsync(UpdateEmployeeRequest updateEmployeeRequest)
    {
        var employee = EmployeeMapper.ToEmployee(updateEmployeeRequest);

        await ValidateEmployeeAsync(employee);
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
    }

    public async Task<string> UpdateEmployeePhotoAsync(Guid id, IFormFile file)
    {
        var employee = await _employeeUnitOfWork.Employee.GetAsync(id) ?? throw new EmployeeNotFoundException();
        string newFileName = FileHelper.GetGuidFileName(Constants.FileExtensionJpg);
        string originalFileName = employee.Photo ?? "";

        await _azureStorageHelper.SaveBlobToAzureStorageContainerAsync(file, Constants.AzureStorageContainerEmployees, newFileName);

        employee.Photo = newFileName;

        await _employeeUnitOfWork.Employee.UpdateAsync(employee);
        await _employeeUnitOfWork.SaveChangesAsync();

        if (string.IsNullOrWhiteSpace(originalFileName))
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

    private async Task<(bool isValid, List<string>? Errors)> ValidateEmployeeAsync(Employee employee)
    {
        var result = await _validator.ValidateAsync(employee, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        return result.IsValid
            ? (true, null)
            : throw new ValidationException(result.Errors);
    }
}