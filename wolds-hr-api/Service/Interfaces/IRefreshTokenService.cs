using wolds_hr_api.Domain;

namespace wolds_hr_api.Service.Interfaces;

public interface IRefreshTokenService
{
    RefreshToken GenerateRefreshToken(string ipAddress, Account account);
}