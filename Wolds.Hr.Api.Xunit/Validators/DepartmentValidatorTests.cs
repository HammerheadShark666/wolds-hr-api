using FluentValidation;
using Moq;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Validator;

namespace wolds_hr_api.xunit.Validators;
public class DepartmentValidatorTests
{
    private readonly Mock<IDepartmentRepository> _departmentRepoMock;
    private readonly DepartmentValidator _validator;

    public DepartmentValidatorTests()
    {
        _departmentRepoMock = new Mock<IDepartmentRepository>();
        _validator = new DepartmentValidator(_departmentRepoMock.Object);
    }

    [Fact]
    public async Task Should_HaveError_WhenNameIsEmpty()
    {
        var department = new Department { Name = "" };

        var result = await _validator.ValidateAsync(department, options => options.IncludeRuleSets("AddUpdate"));

        var nameError = result.Errors.FirstOrDefault(e => e.PropertyName == "Name");

        Assert.NotNull(nameError);
        Assert.Equal("Name is required", nameError.ErrorMessage);
    }

    [Fact]
    public async Task Should_HaveError_WhenNameIsTooLong()
    {
        var department = new Department { Name = new string('A', 51) };

        var result = await _validator.ValidateAsync(department, options => options.IncludeRuleSets("AddUpdate"));

        var nameError = result.Errors.FirstOrDefault(e => e.PropertyName == "Name");

        Assert.NotNull(nameError);
        Assert.Equal("Name must be at most 50 characters long", nameError.ErrorMessage);
    }

    [Fact]
    public async Task Should_HaveError_WhenDepartmentAlreadyExists()
    {
        var department = new Department { Name = "HR" };
        _departmentRepoMock.Setup(r => r.ExistsAsync("HR")).ReturnsAsync(true);

        var result = await _validator.ValidateAsync(department, options => options.IncludeRuleSets("AddUpdate"));

        var nameError = result.Errors.FirstOrDefault(e => e.PropertyName == "Name");

        Assert.NotNull(nameError);
        Assert.Equal("Department already exists", nameError.ErrorMessage);
    }

    [Fact]
    public async Task Should_NotHaveError_WhenNameIsValidAndDoesNotExist()
    {
        var department = new Department { Name = "Finance" };
        _departmentRepoMock.Setup(r => r.ExistsAsync("Finance")).ReturnsAsync(false);

        var context = new ValidationContext<Department>(department)
        {
            RootContextData = { ["RuleSet"] = "AddUpdate" }
        };

        var result = await _validator.ValidateAsync(context);

        Assert.True(result.IsValid);
    }
}