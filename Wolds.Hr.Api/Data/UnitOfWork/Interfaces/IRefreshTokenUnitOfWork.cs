using Wolds.Hr.Api.Data.Interfaces;

namespace Wolds.Hr.Api.Data.UnitOfWork.Interfaces;

internal interface IRefreshTokenUnitOfWork
{
    IRefreshTokenRepository RefreshToken { get; }
    Task SaveChangesAsync();
}
