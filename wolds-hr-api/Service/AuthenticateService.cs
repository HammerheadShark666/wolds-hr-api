using FluentValidation;
using SwanSong.Domain.Dto;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class AuthenticateService(IValidator<LoginRequest> validatorHelper,
                                 IAccountRepository accountRepository,
                                 IRefreshTokenService refreshTokenService) : IAuthenticateService
{
    public readonly IValidator<LoginRequest> _validatorHelper = validatorHelper;
    public readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
    public readonly IAccountRepository _accountRepository = accountRepository;

    #region Public Functions


    public async Task<(bool isValid, Authenticated? authenticated, List<string>? Errors)> AuthenticateAsync(LoginRequest loginRequest) //, string ipAddress
    {

        var result = await _validatorHelper.ValidateAsync(loginRequest, options =>
        {
            options.IncludeRuleSets("LoginValidation");
        });
        if (!result.IsValid)
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());

        var account = GetAccount(loginRequest.Email);

        var jwtToken = AuthenticationHelper.GenerateJwtToken(account,
                                                             EnvironmentVariablesHelper.JwtSettingsTokenExpiryMinutes,
                                                             EnvironmentVariablesHelper.JwtSymmetricSecurityKey);

        return (true, new Authenticated(jwtToken, new Profile(account.FirstName, account.LastName, account.Email)), []); //refreshToken.Token, 
    }

    #endregion

    #region Private Functions

    private Account GetAccount(string email)
    {
        return _accountRepository.Get(email) ?? throw new AppException(ConstantMessages.AccountNotFound);
    }

    #endregion
}