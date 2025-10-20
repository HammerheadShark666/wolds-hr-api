using FluentValidation.Results;

namespace wolds_hr_api.Library.Validation;

internal static class ValidationErrorFormatter
{
    public static IEnumerable<string> ExtractErrorMessages(ValidationResult result) =>
        result.Errors.Select(e => e.ErrorMessage);
}