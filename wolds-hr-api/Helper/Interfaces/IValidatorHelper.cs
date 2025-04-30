namespace wolds_hr_api.Helper.Interfaces;

public interface IValidatorHelper<T>
{
    Task ValidateAsync(T itemToValidate, string ruleSet);
}