namespace wolds_hr_api.Helper.Exceptions;

public class EmployeeImportNotCreated : Exception
{
    public EmployeeImportNotCreated()
    {
    }

    public EmployeeImportNotCreated(string message)
        : base(message)
    {
    }

    public EmployeeImportNotCreated(string message, Exception inner)
        : base(message, inner)
    {
    }
}
