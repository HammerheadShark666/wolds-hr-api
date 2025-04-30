using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IRefreshTokenRepository
{
    bool Exists(string token);
    void Add(RefreshToken refreshToken);
    List<RefreshToken> ById(int accountId);
}
