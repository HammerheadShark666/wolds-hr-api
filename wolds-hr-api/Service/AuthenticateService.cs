using FluentValidation;
using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Dto.Requests;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

internal sealed class AuthenticateService(IValidator<LoginRequest> _validator,
                                 IRefreshTokenService _refreshTokenService,
                                 IAccountUnitOfWork _accountUnitOfWork,
                                 IJWTHelper _jWTHelper) : IAuthenticateService
{
    public async Task<(bool isValid, LoginResponse? authenticated, List<string>? Errors)> AuthenticateAsync(LoginRequest loginRequest, string ipAddress)
    {
        var result = await _validator.ValidateAsync(loginRequest, options =>
        {
            options.IncludeRuleSets("LoginValidation");
        });
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var account = GetAccount(loginRequest.Username);
        var jwtToken = _jWTHelper.GenerateJWTToken(account);
        var refreshToken = _refreshTokenService.GenerateRefreshToken(ipAddress, account);

        _refreshTokenService.RemoveExpiredRefreshTokens(account.Id);
        await _refreshTokenService.AddRefreshTokenAsync(refreshToken);

        return (true, new LoginResponse(jwtToken, refreshToken.Token, new Profile(account.FirstName, account.Surname, account.Email)), []);
    }

    private Account GetAccount(string email)
    {
        return _accountUnitOfWork.Account.Get(email) ?? throw new AccountNotFoundException();
    }
}