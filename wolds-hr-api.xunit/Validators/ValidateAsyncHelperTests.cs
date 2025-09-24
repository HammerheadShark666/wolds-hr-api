using FluentValidation;
using wolds_hr_api.Validator;

namespace wolds_hr_api.xunit.Validators;

public class ValidateAsyncHelperTests
{
    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }

    private class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator()
        {
            RuleSet("TestRules", () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required")
                    .MaximumLength(5).WithMessage("Name too long");
            });
        }
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnValid_WhenModelIsValid()
    {
        var model = new TestModel { Name = "John" };
        var validator = new TestModelValidator();

        var (isValid, errors) = await ValidatorHelper.ValidateAsync(validator, model, "TestRules");

        Assert.True(isValid);
        Assert.Null(errors);
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnErrors_WhenModelIsInvalid()
    {
        var model = new TestModel { Name = "" };
        var validator = new TestModelValidator();

        var (isValid, errors) = await ValidatorHelper.ValidateAsync(validator, model, "TestRules");

        Assert.False(isValid);
        Assert.NotNull(errors);
        Assert.Contains("Name is required", errors!);
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenNameTooLong()
    {
        var model = new TestModel { Name = "TooLongName" };
        var validator = new TestModelValidator();

        var (isValid, errors) = await ValidatorHelper.ValidateAsync(validator, model, "TestRules");

        Assert.False(isValid);
        Assert.NotNull(errors);
        Assert.Contains("Name too long", errors!);
    }
}
