using FluentValidation;
using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Dto.Requests.Department;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Mappers;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class DepartmentService(IValidator<Department> _validator,
                               IDepartmentUnitOfWork _departmentUnitOfWork) : IDepartmentService
{
    public async Task<List<DepartmentResponse>> GetAsync()
    {
        var departments = await _departmentUnitOfWork.Department.GetAsync();
        return DepartmentMapper.ToDepartmentsResponse(departments);
    }

    public async Task<DepartmentResponse?> GetAsync(Guid id)
    {
        var department = await _departmentUnitOfWork.Department.GetAsync(id);
        return department == null ? null : DepartmentMapper.ToDepartmentResponse(department);
    }

    public async Task<DepartmentResponse?> GetAsync(string name)
    {
        var department = await _departmentUnitOfWork.Department.GetAsync(name);
        return department == null ? null : DepartmentMapper.ToDepartmentResponse(department);
    }

    public async Task<(bool isValid, DepartmentResponse? Department, List<string>? Errors)> AddAsync(AddDepartmentRequest addDepartmentRequest)
    {
        var department = DepartmentMapper.ToDepartment(addDepartmentRequest);

        var validation = await ValidateDepartmentAsync(department, "AddUpdate");
        if (!validation.isValid)
            return (false, null, validation.errors);

        _departmentUnitOfWork.Department.Add(department);
        await _departmentUnitOfWork.SaveChangesAsync();

        var newDepartment = await _departmentUnitOfWork.Department.GetAsync(department.Id)
                                        ?? throw new DepartmentNotFoundException("Department not found after adding.");

        return (true, DepartmentMapper.ToDepartmentResponse(newDepartment), null);
    }

    public async Task<(bool isValid, DepartmentResponse? Department, List<string>? Errors)> UpdateAsync(UpdateDepartmentRequest updateDepartmentRequest)
    {
        var department = DepartmentMapper.ToDepartment(updateDepartmentRequest);

        if (!await _departmentUnitOfWork.Department.ExistsAsync(department.Id))
            throw new DepartmentNotFoundException("Department not found.");

        var validation = await ValidateDepartmentAsync(department, "AddUpdate");
        if (!validation.isValid)
            return (false, null, validation.errors);

        await _departmentUnitOfWork.Department.UpdateAsync(department);
        await _departmentUnitOfWork.SaveChangesAsync();

        var updatedDepartment = await _departmentUnitOfWork.Department.GetAsync(department.Id)
            ?? throw new DepartmentNotFoundException("Department not found after updating.");

        return (true, DepartmentMapper.ToDepartmentResponse(updatedDepartment), null);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _departmentUnitOfWork.Department.DeleteAsync(id);
        await _departmentUnitOfWork.SaveChangesAsync();
    }


    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _departmentUnitOfWork.Department.ExistsAsync(id);
    }

    private async Task<(bool isValid, List<string> errors)> ValidateDepartmentAsync(Department department, string ruleSet)
    {
        var result = await _validator.ValidateAsync(department, options =>
        {
            options.IncludeRuleSets(ruleSet);
        });

        return result.IsValid
            ? (true, new List<string>())
            : (false, result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}