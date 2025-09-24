using FluentValidation;
using FluentValidation.Results;

namespace wolds_hr_api.Validator;

public class ValidatorHelper
{
    public static async Task<(bool IsValid, List<string>? Errors)> ValidateAsync<T>(
        IValidator<T> validator,
        T instance,
        string ruleSet)
    {
        ValidationResult result = await validator.ValidateAsync(instance, opts =>
        {
            opts.IncludeRuleSets(ruleSet);
        });

        return result.IsValid
            ? (true, null)
            : (false, result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
