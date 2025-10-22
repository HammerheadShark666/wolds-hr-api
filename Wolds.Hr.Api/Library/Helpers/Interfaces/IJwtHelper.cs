using Wolds.Hr.Api.Domain;

namespace Wolds.Hr.Api.Library.Helpers.Interfaces;

internal interface IJWTHelper
{
    string GenerateJWTToken(Account account);
}
