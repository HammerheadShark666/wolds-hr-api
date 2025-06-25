namespace wolds_hr_api.Helper.Exceptions;

public class EmployeeImportNotFoundException : Exception
{
    public EmployeeImportNotFoundException()
    {
    }

    public EmployeeImportNotFoundException(string message)
        : base(message)
    {
    }

    public EmployeeImportNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
