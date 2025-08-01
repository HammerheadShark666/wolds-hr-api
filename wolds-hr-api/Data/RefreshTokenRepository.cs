﻿using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    private readonly AppDbContext _context = context;

    public async Task<bool> ExistsAsync(string token)
    {
        return await _context.RefreshTokens
                                .AsNoTracking()
                                .AnyAsync(a => a.Token.Equals(token));
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public void Update(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        _context.SaveChanges();
    }

    public void Delete(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Remove(refreshToken);
        _context.SaveChanges();
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
        _context.SaveChanges();
    }
}