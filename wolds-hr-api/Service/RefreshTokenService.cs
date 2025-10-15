using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

internal sealed class RefreshTokenService(IRefreshTokenUnitOfWork _refreshTokenUnitOfWork, IJWTHelper _jWTHelper, IEnvironmentHelper environmentHelper) : IRefreshTokenService
{
    public async Task<JwtRefreshToken> RefreshTokenAsync(string token, string ipAddress)
    {
        var refreshToken = await GetRefreshTokenAsync(token);
        var newRefreshToken = GenerateRefreshToken(ipAddress, refreshToken.Account);

        RemoveExpiredRefreshTokens(refreshToken.Account.Id);
        await AddRefreshTokenAsync(newRefreshToken);

        var jwtToken = _jWTHelper.GenerateJWTToken(refreshToken.Account);

        return new JwtRefreshToken(refreshToken.Account.IsAuthenticated, jwtToken, newRefreshToken.Token,
                                      new Profile(refreshToken.Account.FirstName, refreshToken.Account.Surname,
                                         refreshToken.Account.Email));
    }

    public void RemoveExpiredRefreshTokens(Guid accountId)
    {
        _refreshTokenUnitOfWork.RefreshToken.RemoveExpired(environmentHelper.JWTSettingsRefreshTokenTtl, accountId);
        _refreshTokenUnitOfWork.SaveChangesAsync();
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _refreshTokenUnitOfWork.RefreshToken.AddAsync(refreshToken);
        await _refreshTokenUnitOfWork.SaveChangesAsync();
    }

    public async Task DeleteRefreshTokenAsync(string refreshTokenString)
    {
        var refreshToken = await _refreshTokenUnitOfWork.RefreshToken.ByTokenAsync(refreshTokenString);
        if (refreshToken != null)
        {
            _refreshTokenUnitOfWork.RefreshToken.Delete(refreshToken);
            await _refreshTokenUnitOfWork.SaveChangesAsync();
        }
    }

    public RefreshToken GenerateRefreshToken(string ipAddress, Account account)
    {
        var refreshTokenExpires = DateTime.Now.AddDays(environmentHelper.JWTSettingsRefreshTokenExpiryDays);
        var refreshToken = JWTHelper.GenerateRefreshToken(ipAddress, refreshTokenExpires);
        refreshToken.Account = account;

        return refreshToken;
    }

    private async Task<RefreshToken> GetRefreshTokenAsync(string token)
    {
        var refreshToken = await _refreshTokenUnitOfWork.RefreshToken.ByTokenAsync(token);
        if (refreshToken == null || !refreshToken.IsActive)
        {
            throw new RefreshTokenNotFoundException();
        }

        return refreshToken;
    }
}