namespace wolds_hr_api.Helper.Exceptions;

internal sealed class EnvironmentVariableNotFoundException : Exception
{
    public EnvironmentVariableNotFoundException(string name) : base(string.Format(ConstantMessages.EnvironmentVariableNotFound, name)) { }
}