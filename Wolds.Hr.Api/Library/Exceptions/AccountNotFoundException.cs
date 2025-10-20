namespace wolds_hr_api.Library.Exceptions;

internal sealed class AccountNotFoundException : Exception
{
    public AccountNotFoundException() : base(ConstantMessages.AccountNotFound) { }
}