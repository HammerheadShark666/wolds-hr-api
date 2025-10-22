using Wolds.Hr.Api.Domain;

namespace Wolds.Hr.Api.Data.Interfaces;

public interface IAccountRepository
{
    Account? Get(string email);
}