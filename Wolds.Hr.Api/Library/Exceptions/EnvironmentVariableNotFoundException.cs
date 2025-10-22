namespace Wolds.Hr.Api.Library.Exceptions;

internal sealed class EnvironmentVariableNotFoundException : Exception
{
    public EnvironmentVariableNotFoundException(string name) : base(string.Format(ConstantMessages.EnvironmentVariableNotFound, name)) { }
}