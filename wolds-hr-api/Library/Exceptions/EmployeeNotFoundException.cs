namespace wolds_hr_api.Library.Exceptions;

internal sealed class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException() : base(ConstantMessages.EmployeeNotFound) { }

    public EmployeeNotFoundException(string message)
        : base(message)
    { }
}