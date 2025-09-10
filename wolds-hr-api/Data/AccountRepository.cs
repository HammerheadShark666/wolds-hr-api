using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class AccountRepository(WoldsHrDbContext context) : IAccountRepository
{
    private readonly WoldsHrDbContext _context = context;

    public Account? Get(string email)
    {
        return _context.Accounts.Where(a => a.Email.Equals(email))
                       .FirstOrDefault();
    }
}