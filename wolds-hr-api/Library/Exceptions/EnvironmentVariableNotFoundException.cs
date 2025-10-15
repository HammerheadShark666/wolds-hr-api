namespace wolds_hr_api.Library.Exceptions;

internal sealed class EnvironmentVariableNotFoundException : Exception
{
    public EnvironmentVariableNotFoundException(string name) : base(string.Format(ConstantMessages.EnvironmentVariableNotFound, name)) { }
}