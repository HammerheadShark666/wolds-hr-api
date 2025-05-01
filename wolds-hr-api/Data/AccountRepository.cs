using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;

namespace wolds_hr_api.Data;

public class AccountRepository : IAccountRepository
{
    private static List<Account> accounts = [];

    public AccountRepository()
    {
        if (accounts != null)
            accounts = AccountHelper.CreateAccounts(accounts);
    }

    public Account? Get(string email)
    {
        return accounts.Where(a => a.Email.Equals(email))
                       .FirstOrDefault();
    }
}