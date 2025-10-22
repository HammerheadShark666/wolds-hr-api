namespace Wolds.Hr.Api.Library.Exceptions;

internal sealed class RefreshTokenNotFoundException : Exception
{
    public RefreshTokenNotFoundException() : base(ConstantMessages.InvalidToken) { }
}