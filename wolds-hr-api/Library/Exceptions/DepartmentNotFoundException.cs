namespace wolds_hr_api.Library.Exceptions;

internal sealed class DepartmentNotFoundException : Exception
{
    public DepartmentNotFoundException() : base(ConstantMessages.DocumentNotFound) { }

    public DepartmentNotFoundException(string message)
        : base(message)
    { }
}