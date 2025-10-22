using Wolds.Hr.Api.Library.Dto.Requests;
using Wolds.Hr.Api.Library.Dto.Responses;

namespace Wolds.Hr.Api.Service.Interfaces;

internal interface IAuthenticateService
{
    Task<(bool isValid, LoginResponse? authenticated, List<string>? Errors)> AuthenticateAsync(LoginRequest loginRequest, string ipAddress);
}