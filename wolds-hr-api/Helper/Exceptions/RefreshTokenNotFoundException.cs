namespace wolds_hr_api.Helper.Exceptions;

internal sealed class RefreshTokenNotFoundException : Exception
{
    public RefreshTokenNotFoundException() : base(ConstantMessages.InvalidToken) { }
}