using FluentValidation;
using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Library.Dto.Requests.Department;
using wolds_hr_api.Library.Dto.Responses;
using wolds_hr_api.Library.Exceptions;
using wolds_hr_api.Library.Mappers;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

internal sealed class DepartmentService(IValidator<Department> validator,
                               IDepartmentUnitOfWork departmentUnitOfWork) : IDepartmentService
{
    public async Task<List<DepartmentResponse>> GetAsync()
    {
        var departments = await departmentUnitOfWork.Department.GetAsync();
        return DepartmentMapper.ToDepartmentsResponse(departments);
    }

    public async Task<DepartmentResponse?> GetAsync(Guid id)
    {
        var department = await departmentUnitOfWork.Department.GetAsync(id);
        return department == null ? null : DepartmentMapper.ToDepartmentResponse(department);
    }

    public async Task<DepartmentResponse?> GetAsync(string name)
    {
        var department = await departmentUnitOfWork.Department.GetAsync(name);
        return department == null ? null : DepartmentMapper.ToDepartmentResponse(department);
    }

    public async Task<(bool isValid, DepartmentResponse? Department, List<string>? Errors)> AddAsync(AddDepartmentRequest addDepartmentRequest)
    {
        var department = DepartmentMapper.ToDepartment(addDepartmentRequest);

        await ValidateDepartmentAsync(department);

        departmentUnitOfWork.Department.Add(department);
        await departmentUnitOfWork.SaveChangesAsync();

        var newDepartment = await departmentUnitOfWork.Department.GetAsync(department.Id)
                                        ?? throw new DepartmentNotFoundException("Department not found after adding.");

        return (true, DepartmentMapper.ToDepartmentResponse(newDepartment), null);
    }

    public async Task<(bool isValid, DepartmentResponse? Department, List<string>? Errors)> UpdateAsync(UpdateDepartmentRequest updateDepartmentRequest)
    {
        var department = DepartmentMapper.ToDepartment(updateDepartmentRequest);

        if (!await departmentUnitOfWork.Department.ExistsAsync(department.Id))
            throw new DepartmentNotFoundException();

        await ValidateDepartmentAsync(department);

        await departmentUnitOfWork.Department.UpdateAsync(department);
        await departmentUnitOfWork.SaveChangesAsync();

        var updatedDepartment = await departmentUnitOfWork.Department.GetAsync(department.Id)
            ?? throw new DepartmentNotFoundException("Department not found after updating.");

        return (true, DepartmentMapper.ToDepartmentResponse(updatedDepartment), null);
    }

    public async Task DeleteAsync(Guid id)
    {
        await departmentUnitOfWork.Department.DeleteAsync(id);
        await departmentUnitOfWork.SaveChangesAsync();
    }


    public async Task<bool> ExistsAsync(Guid id)
    {
        return await departmentUnitOfWork.Department.ExistsAsync(id);
    }

    private async Task<(bool isValid, List<string> errors)> ValidateDepartmentAsync(Department department)
    {
        var result = await validator.ValidateAsync(department, options =>
        {
            options.IncludeRuleSets("AddUpdate");
        });

        return result.IsValid
            ? (true, [])
            : throw new ValidationException(result.Errors);
    }
}