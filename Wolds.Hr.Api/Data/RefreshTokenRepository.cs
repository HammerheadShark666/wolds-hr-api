using Microsoft.EntityFrameworkCore;
using Wolds.Hr.Api.Data.Context;
using Wolds.Hr.Api.Data.Interfaces;
using Wolds.Hr.Api.Domain;

namespace Wolds.Hr.Api.Data;

internal sealed class RefreshTokenRepository(WoldsHrDbContext woldsHrDbContext) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken refreshToken)
    {
        await woldsHrDbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public void Delete(RefreshToken refreshToken)
    {
        woldsHrDbContext.RefreshTokens.Remove(refreshToken);
    }

    public async Task<RefreshToken?> ByTokenAsync(string token)
    {
        return await woldsHrDbContext.RefreshTokens
                             .Include(a => a.Account)
                             .Where(x => x.Token.Equals(token))
                             .SingleOrDefaultAsync();
    }

    public async Task<List<RefreshToken>> ByIdAsync(Guid accountId)
    {
        return await woldsHrDbContext.RefreshTokens.Where(a => a.Account.Id.Equals(accountId)).ToListAsync();
    }

    public void RemoveExpired(int expireDays, Guid accountId)
    {
        var refreshTokens = woldsHrDbContext.RefreshTokens.Where(a => a.Account.Id.Equals(accountId)
                                                            && DateTime.Now >= a.Expires
                                                                && a.Created.AddDays(expireDays) <= DateTime.Now).ToList();

        woldsHrDbContext.RefreshTokens.RemoveRange(refreshTokens);
    }
}