using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Requests;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class AuthenticateService(IValidator<LoginRequest> validatorHelper,
                                 IAccountRepository accountRepository,
                                 IJwtHelper jwtHelper) : IAuthenticateService
{
    public readonly IValidator<LoginRequest> _validatorHelper = validatorHelper;
    public readonly IAccountRepository _accountRepository = accountRepository;
    public readonly IJwtHelper _jwtHelper = jwtHelper;

    #region Public Functions


    public async Task<(bool isValid, AuthenticatedResponse? authenticated, List<string>? Errors)> AuthenticateAsync(LoginRequest loginRequest)
    {

        var result = await _validatorHelper.ValidateAsync(loginRequest, options =>
        {
            options.IncludeRuleSets("LoginValidation");
        });
        if (!result.IsValid)
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());

        var account = GetAccount(loginRequest.Email);
        var jwtToken = _jwtHelper.GenerateJwtToken(account);

        return (true, new AuthenticatedResponse(jwtToken, new Profile(account.FirstName, account.LastName, account.Email)), []);
    }

    #endregion

    #region Private Functions

    private Account GetAccount(string email)
    {
        return _accountRepository.Get(email) ?? throw new AppException(ConstantMessages.AccountNotFound);
    }

    #endregion
}