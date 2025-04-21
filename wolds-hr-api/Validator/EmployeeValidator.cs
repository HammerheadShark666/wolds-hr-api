using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

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
                .NotEmpty()
                .MinimumLength(3)
            .WithMessage("Surname must be at least 3 characters long.");

            RuleFor(employee => employee.FirstName)
                .NotEmpty()
                .MinimumLength(3)
            .WithMessage("First name must be at least 3 characters long.");

        });
    }

    protected bool EmployeeExists(int id)
    {
        return _employeeRepository.Exists(id);
    }
}