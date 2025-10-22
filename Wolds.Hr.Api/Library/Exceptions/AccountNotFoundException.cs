namespace Wolds.Hr.Api.Library.Exceptions;

internal sealed class AccountNotFoundException : Exception
{
    public AccountNotFoundException() : base(ConstantMessages.AccountNotFound) { }
}