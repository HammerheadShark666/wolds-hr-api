using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class RefreshTokenService(IRefreshTokenRepository refreshTokenRepository) : IRefreshTokenService
{
    public readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

    public RefreshToken GenerateRefreshToken(string ipAddress, Account account)
    {
        var refreshTokenExpires = DateTime.Now.AddDays(EnvironmentVariablesHelper.JwtSettingsRefreshTokenExpiryDays);
        var refreshToken = AuthenticationHelper.GenerateRefreshToken(ipAddress, refreshTokenExpires);
        refreshToken.Account = account;

        return refreshToken;
    }
}