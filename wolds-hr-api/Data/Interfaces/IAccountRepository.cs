using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IAccountRepository
{
    Account? Get(string email);
}