using Wolds.Hr.Api.Data.Context;
using Wolds.Hr.Api.Data.Interfaces;
using Wolds.Hr.Api.Domain;

namespace Wolds.Hr.Api.Data;

internal sealed class AccountRepository(WoldsHrDbContext woldsHrDbContext) : IAccountRepository
{
    public Account? Get(string email)
    {
        return woldsHrDbContext.Accounts.Where(a => a.Email.Equals(email))
                       .FirstOrDefault();
    }
}