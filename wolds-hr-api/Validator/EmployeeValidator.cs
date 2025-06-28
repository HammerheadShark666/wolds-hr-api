using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;

namespace wolds_hr_api.Validator;

public class EmployeeValidator : AbstractValidator<Employee>
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeValidator(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;

        RuleSet("AddUpdate", () =>
        {
            RuleFor(employee => employee.Surname)
                .MinimumLength(2)
            .WithMessage("Surname must be at least 2 characters long.");

            RuleFor(employee => employee.FirstName)
                .NotEmpty()
                .MinimumLength(3)
            .WithMessage("First name must be at least 3 characters long.");

            RuleFor(_ => _)
                .MustAsync(async (employee, cancellation) =>
                {
                    return await NumberOfEmployeesWithinMax();
                })
                .WithMessage($"Maximum number of employees reached: {Constants.MaxNumberOfEmployees}");
        });
    }

    protected async Task<bool> NumberOfEmployeesWithinMax()
    {
        return !(await _employeeRepository.CountAsync() > Constants.MaxNumberOfEmployees);
    }
}