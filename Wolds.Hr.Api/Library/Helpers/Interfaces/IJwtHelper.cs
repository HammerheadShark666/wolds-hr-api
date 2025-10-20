using wolds_hr_api.Domain;

namespace wolds_hr_api.Library.Helpers.Interfaces;

internal interface IJWTHelper
{
    string GenerateJWTToken(Account account);
}
