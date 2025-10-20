namespace wolds_hr_api.Library.Exceptions;

internal sealed class RefreshTokenNotFoundException : Exception
{
    public RefreshTokenNotFoundException() : base(ConstantMessages.InvalidToken) { }
}