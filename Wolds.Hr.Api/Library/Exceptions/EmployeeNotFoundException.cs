namespace Wolds.Hr.Api.Library.Exceptions;

internal sealed class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException() : base(ConstantMessages.EmployeeNotFound) { }

    public EmployeeNotFoundException(string message)
        : base(message)
    { }
}