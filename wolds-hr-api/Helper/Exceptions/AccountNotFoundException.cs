namespace wolds_hr_api.Helper.Exceptions;

internal sealed class AccountNotFoundException : Exception
{
    public AccountNotFoundException() : base(ConstantMessages.AccountNotFound) { }
}
