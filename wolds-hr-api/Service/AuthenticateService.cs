using FluentValidation;
using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Requests;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class AuthenticateService(IValidator<LoginRequest> _validatorHelper,
                                 IRefreshTokenService _refreshTokenService,
                                 IAccountUnitOfWork _accountUnitOfWork,
                                 IJWTHelper _jWTHelper) : IAuthenticateService
{
    #region Public Functions 

    public async Task<(bool isValid, LoginResponse? authenticated, List<string>? Errors)> AuthenticateAsync(LoginRequest loginRequest, string ipAddress)
    {

        var result = await _validatorHelper.ValidateAsync(loginRequest, options =>
        {
            options.IncludeRuleSets("LoginValidation");
        });
        if (!result.IsValid)
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());

        var account = GetAccount(loginRequest.Username);
        var jwtToken = _jWTHelper.GenerateJWTToken(account);
        var refreshToken = _refreshTokenService.GenerateRefreshToken(ipAddress, account);

        _refreshTokenService.RemoveExpiredRefreshTokens(account.Id);
        await _refreshTokenService.AddRefreshTokenAsync(refreshToken);

        return (true, new LoginResponse(jwtToken, refreshToken.Token, new Profile(account.FirstName, account.Surname, account.Email)), []);
    }

    public async Task<JwtRefreshToken> RefreshTokenAsync(string token, string ipAddress)
    {
        var refreshToken = await _refreshTokenService.GetRefreshTokenAsync(token);
        var newRefreshToken = _refreshTokenService.GenerateRefreshToken(ipAddress, refreshToken.Account);

        _refreshTokenService.RemoveExpiredRefreshTokens(refreshToken.Account.Id);
        await _refreshTokenService.AddRefreshTokenAsync(newRefreshToken);

        var jwtToken = _jWTHelper.GenerateJWTToken(refreshToken.Account);

        return new JwtRefreshToken(refreshToken.Account.IsAuthenticated, jwtToken, newRefreshToken.Token,
                                      new Profile(refreshToken.Account.FirstName, refreshToken.Account.Surname,
                                         refreshToken.Account.Email));
    }

    #endregion

    #region Private Functions

    private Account GetAccount(string email)
    {
        return _accountUnitOfWork.Account.Get(email) ?? throw new AppException(ConstantMessages.AccountNotFound);
    }

    #endregion
}