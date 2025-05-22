using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class AccountRepository : IAccountRepository
{
    private static List<Account> accounts = [];

    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public Account? Get(string email)
    {
        return _context.Accounts.Where(a => a.Email.Equals(email))
                       .FirstOrDefault();
    }
}