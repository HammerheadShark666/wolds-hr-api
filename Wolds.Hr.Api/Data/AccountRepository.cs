using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

internal sealed class AccountRepository(WoldsHrDbContext woldsHrDbContext) : IAccountRepository
{
    public Account? Get(string email)
    {
        return woldsHrDbContext.Accounts.Where(a => a.Email.Equals(email))
                       .FirstOrDefault();
    }
}