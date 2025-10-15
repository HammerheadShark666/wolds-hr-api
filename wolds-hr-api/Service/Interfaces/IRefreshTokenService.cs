using wolds_hr_api.Domain;
using wolds_hr_api.Library.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

internal interface IRefreshTokenService
{
    Task<JwtRefreshToken> RefreshTokenAsync(string token, string ipAddress);
    void RemoveExpiredRefreshTokens(Guid accountId);
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task DeleteRefreshTokenAsync(string refreshTokenString);
    RefreshToken GenerateRefreshToken(string ipAddress, Account account);
}