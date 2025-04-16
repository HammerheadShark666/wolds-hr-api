using employee_test_api.Domain;
using FluentValidation;

namespace employee_test_api.Validator;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(employee => employee.Surname)
          .NotEmpty()
          .MinimumLength(3)
          .WithMessage("Surname must be at least 3 characters long.");

        RuleFor(employee => employee.FirstName)
          .NotEmpty()
          .MinimumLength(3)
          .WithMessage("First name must be at least 3 characters long.");
    }
}