using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class RefreshTokenService(IRefreshTokenUnitOfWork _refreshTokenUnitOfWork) : IRefreshTokenService
{
    public void RemoveExpiredRefreshTokens(Guid accountId)
    {
        _refreshTokenUnitOfWork.RefreshToken.RemoveExpired(EnvironmentVariablesHelper.JWTSettingsRefreshTokenTtl, accountId);
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
        var refreshTokenExpires = DateTime.Now.AddDays(EnvironmentVariablesHelper.JWTSettingsRefreshTokenExpiryDays);
        var refreshToken = JWTHelper.GenerateRefreshToken(ipAddress, refreshTokenExpires);
        refreshToken.Account = account;

        return refreshToken;
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(string token)
    {
        var refreshToken = await _refreshTokenUnitOfWork.RefreshToken.ByTokenAsync(token);
        if (refreshToken == null || !refreshToken.IsActive)
        {
            throw new RefreshTokenNotFoundException(ConstantMessages.InvalidToken);
        }

        return refreshToken;
    }
}