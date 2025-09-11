using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Validator;

public class ImportEmployeeValidator : AbstractValidator<Employee>
{
    public ImportEmployeeValidator(IDepartmentRepository departmentRepository)
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

        //RuleFor(x => x.HireDate)
        //    .Must(BeValidHireDate).WithMessage("Hire date must be in YYYY-MM-DD format, after Jan 1, 2000 and not in the future")
        //    .When(x => !string.IsNullOrWhiteSpace(x.HireDate));

        RuleFor(x => x.DateOfBirth)
            .Must(date => date == null || (date >= new DateOnly(2000, 1, 1) && date <= DateOnly.FromDateTime(DateTime.UtcNow)))
            .WithMessage("Date of birth must be in YYYY-MM-DD format, after Jan 1, 1950 and before Jan 1, 2007");

        //RuleFor(x => x.DateOfBirth)
        //    .Must(BeVaessage("Date of birth must be in YYYY-MM-DD format, after Jan 1, 1950 and before Jan 1, 2007")
        //    .When(x => !string.IsNullOrWhiteSpaclidDob).WithMe(x.DateOfBirth));

        RuleFor(x => x.DepartmentId)
            .MustAsync(async (deptId, cancellation) =>
            {
                if (deptId == null) return true; // optional

                return departmentRepository.Exists(deptId.Value);
            })
            .WithMessage("Department does not exist in database");
    }
}
