using wolds_hr_api.Data.Interfaces;

namespace wolds_hr_api.Data.UnitOfWork.Interfaces;

internal interface IRefreshTokenUnitOfWork
{
    IRefreshTokenRepository RefreshToken { get; }
    Task SaveChangesAsync();
}
