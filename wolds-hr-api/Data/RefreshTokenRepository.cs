using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class RefreshTokenRepository(WoldsHrDbContext context) : IRefreshTokenRepository
{
    private readonly WoldsHrDbContext _context = context;

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
    }

    public void Delete(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Remove(refreshToken);
    }

    public async Task<RefreshToken?> ByTokenAsync(string token)
    {
        return await _context.RefreshTokens
                             .Include(a => a.Account)
                             .Where(x => x.Token.Equals(token))
                             .SingleOrDefaultAsync();
    }

    public async Task<List<RefreshToken>> ByIdAsync(Guid accountId)
    {
        return await _context.RefreshTokens.Where(a => a.Account.Id.Equals(accountId)).ToListAsync();
    }

    public void RemoveExpired(int expireDays, Guid accountId)
    {
        var refreshTokens = _context.RefreshTokens.Where(a => a.Account.Id.Equals(accountId)
                                                            && DateTime.Now >= a.Expires
                                                                && a.Created.AddDays(expireDays) <= DateTime.Now).ToList();

        _context.RefreshTokens.RemoveRange(refreshTokens);
    }
}