using FluentValidation;
using wolds_hr_api.Helper.Interfaces;

namespace wolds_hr_api.Helper;
public class ValidatorHelper<T> : IValidatorHelper<T>
{
    private readonly IValidator<T> _validator;
    public ValidatorHelper(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async Task ValidateAsync(T itemToValidate, string ruleSet)
    {
        var validationResult = await _validator.ValidateAsync(itemToValidate, options => options.IncludeRuleSets(ruleSet)); ;
        if (!validationResult.IsValid)
        {
            //throw new FailedValidationException(new FailedValidationResponse(validationResult.Errors, false));
        }
    }
}