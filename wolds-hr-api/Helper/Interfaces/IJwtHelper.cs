using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper.Interfaces;

public interface IJWTHelper
{
    string GenerateJWTToken(Account account);
}
