using FluentValidation;
using Wolds.Hr.Api.Data.UnitOfWork.Interfaces;
using Wolds.Hr.Api.Domain;
using Wolds.Hr.Api.Library;
using Wolds.Hr.Api.Library.Dto.Requests.Employee;
using Wolds.Hr.Api.Library.Dto.Responses;
using Wolds.Hr.Api.Library.Exceptions;
using Wolds.Hr.Api.Library.Helpers.Interfaces;
using Wolds.Hr.Api.Library.Mappers;
using Wolds.Hr.Api.Service.Interfaces;
using static Wolds.Hr.Api.Library.Helpers.PhotoHelper;

namespace Wolds.Hr.Api.Service;

internal sealed class EmployeeService(IValidator<Employee> validator,
                                      IEmployeeUnitOfWork employeeUnitOfWork,
                                      IAzureStorageBlobHelper azureStorageHelper,
                                      IFileHelper fileHelper,
                                      IPhotoHelper photoHelper) : IEmployeeService
{
    public async Task<EmployeePagedResponse> SearchAsync(string keyword, Guid? departmentId, int page, int pageSize)
    {
        var (employees, totalEmployees) = await employeeUnitOfWork.Employee.GetAsync(keyword, departmentId, page, pageSize);

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
        var employee = await employeeUnitOfWork.Employee.GetAsync(id);
        return employee == null ? null : EmployeeMapper.ToEmployeeResponse(employee);
    }

    public async Task<(bool isValid, EmployeeResponse? Employee, List<string>? Errors)> AddAsync(AddEmployeeRequest addEmployeeRequest)
    {
        var employee = EmployeeMapper.ToEmployee(addEmployeeRequest);

        await ValidateEmployeeAsync(employee);
        employeeUnitOfWork.Employee.Add(employee);
        await employeeUnitOfWork.SaveChangesAsync();

        var newEmployee = await employeeUnitOfWork.Employee.GetAsync(employee.Id)
          ?? throw new EmployeeNotFoundException("Employee not found after adding.");

        return (true, EmployeeMapper.ToEmployeeResponse(newEmployee), null);
    }

    public async Task<(bool isValid, EmployeeResponse? Employee, List<string>? Errors)> UpdateAsync(UpdateEmployeeRequest updateEmployeeRequest)
    {
        var employee = EmployeeMapper.ToEmployee(updateEmployeeRequest);

        await ValidateEmployeeAsync(employee);
        await employeeUnitOfWork.Employee.UpdateAsync(employee);
        await employeeUnitOfWork.SaveChangesAsync();

        var updatedEmployee = await employeeUnitOfWork.Employee.GetAsync(employee.Id)
            ?? throw new EmployeeNotFoundException("Employee not found after updating.");

        return (true, EmployeeMapper.ToEmployeeResponse(updatedEmployee), null);
    }

    public async Task DeleteAsync(Guid id)
    {
        await employeeUnitOfWork.Employee.DeleteAsync(id);
        await employeeUnitOfWork.SaveChangesAsync();
    }

    public async Task<string> UpdateEmployeePhotoAsync(Guid id, IFormFile file)
    {
        var employee = await employeeUnitOfWork.Employee.GetAsync(id) ?? throw new EmployeeNotFoundException();
        string newFileName = fileHelper.GetGuidFileName(Constants.FileExtensionJpg);
        string originalFileName = employee.Photo ?? "";

        await azureStorageHelper.SaveBlobToAzureStorageContainerAsync(file, Constants.AzureStorageContainerEmployees, newFileName);

        employee.Photo = newFileName;

        await employeeUnitOfWork.Employee.UpdateAsync(employee);
        await employeeUnitOfWork.SaveChangesAsync();

        if (string.IsNullOrWhiteSpace(originalFileName))
            await DeleteOriginalFileAsync(originalFileName, newFileName, Constants.AzureStorageContainerEmployees);

        return newFileName;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await employeeUnitOfWork.Employee.ExistsAsync(id);
    }

    private async Task DeleteOriginalFileAsync(string originalFileName, string newFileName, string container)
    {
        EditPhoto editPhoto = photoHelper.WasPhotoEdited(originalFileName, newFileName, Constants.DefaultEmployeePhotoFileName);
        if (editPhoto.PhotoWasChanged)
            await azureStorageHelper.DeleteBlobInAzureStorageContainerAsync(editPhoto.OriginalPhotoName, container);
    }

    private async Task<(bool isValid, List<string>? Errors)> ValidateEmployeeAsync(Employee employee)
    {
        var result = await validator.ValidateAsync(employee, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        return result.IsValid
            ? (true, null)
            : throw new ValidationException(result.Errors);
    }
}