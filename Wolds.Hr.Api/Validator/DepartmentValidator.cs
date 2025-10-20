using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Validator;

public sealed class DepartmentValidator : AbstractValidator<Department>
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentValidator(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;

        RuleSet("AddUpdate", () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name must be at most 50 characters long");

            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellation) =>
                {
                    return !await _departmentRepository.ExistsAsync(name);
                })
                .WithMessage("Department already exists");
        });
    }
}