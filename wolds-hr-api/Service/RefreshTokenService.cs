using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class RefreshTokenService(IRefreshTokenRepository refreshTokenRepository) : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

    public void RemoveExpiredRefreshTokens(int accountId)
    {
        _refreshTokenRepository.RemoveExpired(EnvironmentVariablesHelper.JWTSettingsRefreshTokenTtl, accountId);
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _refreshTokenRepository.AddAsync(refreshToken);
    }

    public async Task DeleteRefreshTokenAsync(string refreshTokenString)
    {
        var refreshToken = await _refreshTokenRepository.ByTokenAsync(refreshTokenString);
        if (refreshToken != null)
        {
            _refreshTokenRepository.Delete(refreshToken);
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
        var refreshToken = await _refreshTokenRepository.ByTokenAsync(token);
        if (refreshToken == null || !refreshToken.IsActive)
        {
            throw new RefreshTokenNotFoundException(ConstantMessages.InvalidToken);
        }

        return refreshToken;
    }
}