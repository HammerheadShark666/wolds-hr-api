using FluentValidation;
using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Library.Dto.Requests;
using wolds_hr_api.Library.Dto.Responses;
using wolds_hr_api.Library.Exceptions;
using wolds_hr_api.Library.Helpers.Interfaces;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

internal sealed class AuthenticateService(IValidator<LoginRequest> validator,
                                          IRefreshTokenService refreshTokenService,
                                          IAccountUnitOfWork accountUnitOfWork,
                                          IJWTHelper jwtHelper) : IAuthenticateService
{
    public async Task<(bool isValid, LoginResponse? authenticated, List<string>? Errors)> AuthenticateAsync(LoginRequest loginRequest, string ipAddress)
    {
        var result = await validator.ValidateAsync(loginRequest, options =>
        {
            options.IncludeRuleSets("LoginValidation");
        });
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var account = GetAccount(loginRequest.Username);
        var jwtToken = jwtHelper.GenerateJWTToken(account);
        var refreshToken = refreshTokenService.GenerateRefreshToken(ipAddress, account);

        refreshTokenService.RemoveExpiredRefreshTokens(account.Id);
        await refreshTokenService.AddRefreshTokenAsync(refreshToken);

        return (true, new LoginResponse(jwtToken, refreshToken.Token, new Profile(account.FirstName, account.Surname, account.Email)), []);
    }

    private Account GetAccount(string email)
    {
        return accountUnitOfWork.Account.Get(email) ?? throw new AccountNotFoundException();
    }
}