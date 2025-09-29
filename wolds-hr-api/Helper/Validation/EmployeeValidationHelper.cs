using FluentValidation;
using FluentValidation.Results;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper.Validation;

public static class EmployeeValidationHelper
{
    public static async Task<ValidationResult> ValidateEmployeeAsync(
        Employee employee, IValidator<Employee> validator)
    {
        return await validator.ValidateAsync(employee, opts => opts.IncludeRuleSets("AddUpdate"));
    }
}