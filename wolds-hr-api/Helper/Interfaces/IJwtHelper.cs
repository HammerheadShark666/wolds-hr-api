using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper.Interfaces;

public interface IJwtHelper
{
    string GenerateJwtToken(Account account);
}
