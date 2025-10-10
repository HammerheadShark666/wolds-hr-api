namespace wolds_hr_api.Helper.Exceptions;

internal sealed class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException() : base(ConstantMessages.EmployeeNotFound) { }

    public EmployeeNotFoundException(string message)
        : base(message)
    { }
}
