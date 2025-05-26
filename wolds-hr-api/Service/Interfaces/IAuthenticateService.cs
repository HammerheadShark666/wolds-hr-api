using wolds_hr_api.Helper.Dto.Requests;
using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IAuthenticateService
{
    Task<(bool isValid, LoginResponse? authenticated, List<string>? Errors)> AuthenticateAsync(LoginRequest loginRequest, string ipAddress);
    Task<JwtRefreshToken> RefreshTokenAsync(string token, string ipAddress);
}