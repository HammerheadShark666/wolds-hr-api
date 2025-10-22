using Wolds.Hr.Api.Domain;
using Wolds.Hr.Api.Library.Dto.Responses;

namespace Wolds.Hr.Api.Service.Interfaces;

internal interface IRefreshTokenService
{
    Task<JwtRefreshToken> RefreshTokenAsync(string token, string ipAddress);
    void RemoveExpiredRefreshTokens(Guid accountId);
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task DeleteRefreshTokenAsync(string refreshTokenString);
    RefreshToken GenerateRefreshToken(string ipAddress, Account account);
}