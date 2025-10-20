using FluentValidation.Results;
using wolds_hr_api.Library.Validation;


namespace wolds_hr_api.xunit.Validators;

public class ValidationErrorFormatterTest
{
    [Fact]
    public void ExtractErrorMessages_ReturnsAllErrorMessages()
    {
        var errors = new List<ValidationFailure>
        {
            new("Name", "Name is required"),
            new("Email", "Email is not valid")
        };
        var validationResult = new ValidationResult(errors);

        var result = ValidationErrorFormatter.ExtractErrorMessages(validationResult);

        var messages = result.ToList();
        Assert.Equal(2, messages.Count);
        Assert.Contains("Name is required", messages);
        Assert.Contains("Email is not valid", messages);
    }

    [Fact]
    public void ExtractErrorMessages_WithNoErrors_ReturnsEmptyEnumerable()
    {
        var validationResult = new ValidationResult();

        var result = ValidationErrorFormatter.ExtractErrorMessages(validationResult);

        Assert.Empty(result);
    }
}
