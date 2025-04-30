using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class RefreshTokenRepository() : IRefreshTokenRepository
{
    private static readonly List<RefreshToken> refreshTokens = [];

    public bool Exists(string token)
    {
        return refreshTokens.Any(a => a.Token.Equals(token));
    }

    public void Add(RefreshToken refreshToken)
    {
        refreshTokens.Add(refreshToken);
    }

    public List<RefreshToken> ById(int accountId)
    {
        return refreshTokens.Where(a => a.Account.Id.Equals(accountId)).ToList();
    }
}