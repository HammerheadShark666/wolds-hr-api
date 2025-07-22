using wolds_hr_api.Domain;

namespace wolds_hr_api.Service.Interfaces;

public interface IRefreshTokenService
{
    void RemoveExpiredRefreshTokens(Guid accountId);
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task DeleteRefreshTokenAsync(string refreshTokenString);
    RefreshToken GenerateRefreshToken(string ipAddress, Account account);
    Task<RefreshToken> GetRefreshTokenAsync(string token);
}