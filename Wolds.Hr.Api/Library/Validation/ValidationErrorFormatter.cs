using FluentValidation.Results;

namespace Wolds.Hr.Api.Library.Validation;

internal static class ValidationErrorFormatter
{
    public static IEnumerable<string> ExtractErrorMessages(ValidationResult result) =>
        result.Errors.Select(e => e.ErrorMessage);
}