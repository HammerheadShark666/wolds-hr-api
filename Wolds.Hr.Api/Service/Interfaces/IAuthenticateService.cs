using wolds_hr_api.Library.Dto.Requests;
using wolds_hr_api.Library.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

internal interface IAuthenticateService
{
    Task<(bool isValid, LoginResponse? authenticated, List<string>? Errors)> AuthenticateAsync(LoginRequest loginRequest, string ipAddress);
}