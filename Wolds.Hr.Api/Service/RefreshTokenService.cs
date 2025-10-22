using Wolds.Hr.Api.Data.UnitOfWork.Interfaces;
using Wolds.Hr.Api.Domain;
using Wolds.Hr.Api.Library.Dto.Responses;
using Wolds.Hr.Api.Library.Exceptions;
using Wolds.Hr.Api.Library.Helpers;
using Wolds.Hr.Api.Library.Helpers.Interfaces;
using Wolds.Hr.Api.Service.Interfaces;

namespace Wolds.Hr.Api.Service;

internal sealed class RefreshTokenService(IRefreshTokenUnitOfWork refreshTokenUnitOfWork, IJWTHelper jwtHelper, IEnvironmentHelper environmentHelper) : IRefreshTokenService
{
    public async Task<JwtRefreshToken> RefreshTokenAsync(string token, string ipAddress)
    {
        var refreshToken = await GetRefreshTokenAsync(token);
        var newRefreshToken = GenerateRefreshToken(ipAddress, refreshToken.Account);

        RemoveExpiredRefreshTokens(refreshToken.Account.Id);
        await AddRefreshTokenAsync(newRefreshToken);

        var jwtToken = jwtHelper.GenerateJWTToken(refreshToken.Account);

        return new JwtRefreshToken(refreshToken.Account.IsAuthenticated, jwtToken, newRefreshToken.Token,
                                      new Profile(refreshToken.Account.FirstName, refreshToken.Account.Surname,
                                         refreshToken.Account.Email));
    }

    public void RemoveExpiredRefreshTokens(Guid accountId)
    {
        refreshTokenUnitOfWork.RefreshToken.RemoveExpired(environmentHelper.JWTSettingsRefreshTokenTtl, accountId);
        refreshTokenUnitOfWork.SaveChangesAsync();
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await refreshTokenUnitOfWork.RefreshToken.AddAsync(refreshToken);
        await refreshTokenUnitOfWork.SaveChangesAsync();
    }

    public async Task DeleteRefreshTokenAsync(string refreshTokenString)
    {
        var refreshToken = await refreshTokenUnitOfWork.RefreshToken.ByTokenAsync(refreshTokenString);
        if (refreshToken != null)
        {
            refreshTokenUnitOfWork.RefreshToken.Delete(refreshToken);
            await refreshTokenUnitOfWork.SaveChangesAsync();
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
        var refreshToken = await refreshTokenUnitOfWork.RefreshToken.ByTokenAsync(token);
        if (refreshToken == null || !refreshToken.IsActive)
        {
            throw new RefreshTokenNotFoundException();
        }

        return refreshToken;
    }
}