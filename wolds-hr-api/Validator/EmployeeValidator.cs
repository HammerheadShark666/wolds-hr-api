using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;

namespace wolds_hr_api.Validator;

public class EmployeeValidator : AbstractValidator<Employee>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public EmployeeValidator(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;

        RuleSet("AddUpdate", () =>
        {
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required")
                .MaximumLength(25).WithMessage("Surname must be at most 25 characters long");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(25).WithMessage("First name must be at most 25 characters long");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(25).WithMessage("Phone number must be less than or equal to 25 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(250).WithMessage("Email must be at most 250 characters long")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.HireDate)
                .Must(date => date == null || (date >= new DateOnly(2000, 1, 1) && date <= DateOnly.FromDateTime(DateTime.UtcNow)))
                .WithMessage("Hire date must be after Jan 1, 2000 and not in the future");

            RuleFor(x => x.DateOfBirth)
                .Must(date => date == null || (date >= new DateOnly(1950, 1, 1) && date <= new DateOnly(2005, 1, 1)))
                .WithMessage("Date of birth must be in YYYY-MM-DD format, after Jan 1, 1950 and before Jan 1, 2005");

            RuleFor(x => x.DepartmentId)
                .MustAsync(async (deptId, cancellation) =>
                {
                    return deptId == null || await _departmentRepository.ExistsAsync(deptId.Value);
                })
                .WithMessage("Department does not exist in database");

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