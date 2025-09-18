using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Data.UnitOfWork.Interfaces;

namespace wolds_hr_api.Data.UnitOfWork;

public class RefreshTokenUnitOfWork(IRefreshTokenRepository refreshToken,
                                    WoldsHrDbContext dbContext) : IRefreshTokenUnitOfWork
{
    public IRefreshTokenRepository RefreshToken { get; } = refreshToken;

    private readonly WoldsHrDbContext _dbContext = dbContext;
    public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
}