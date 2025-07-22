using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IRefreshTokenRepository
{
    Task<bool> ExistsAsync(string token);
    Task AddAsync(RefreshToken refreshToken);
    void Update(RefreshToken refreshToken);
    void Delete(RefreshToken refreshToken);
    Task<List<RefreshToken>> ByIdAsync(Guid accountId);
    Task<RefreshToken?> ByTokenAsync(string token);
    void RemoveExpired(int expireDays, Guid accountId);
}
