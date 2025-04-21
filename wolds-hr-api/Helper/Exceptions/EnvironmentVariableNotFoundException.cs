namespace wolds_hr_api.Helper.Exceptions;

public class EnvironmentVariableNotFoundException : Exception
{
    public EnvironmentVariableNotFoundException()
    {
    }

    public EnvironmentVariableNotFoundException(string message)
        : base(message)
    {
    }

    public EnvironmentVariableNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}