namespace wolds_hr_api.Helper.Exceptions;

public class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException()
    {
    }

    public EmployeeNotFoundException(string message)
        : base(message)
    {
    }

    public EmployeeNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
