using SwanSong.Domain.Dto;

namespace wolds_hr_api.Service.Interfaces;

public interface IAuthenticateService
{
    Task<(bool isValid, Authenticated? authenticated, List<string>? Errors)> AuthenticateAsync(LoginRequest loginRequest);
}