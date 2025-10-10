using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper.Interfaces;

internal interface IJWTHelper
{
    string GenerateJWTToken(Account account);
}
