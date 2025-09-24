namespace wolds_hr_api.Helper.Exceptions;

public class DepartmentNotFoundException : Exception
{
    public DepartmentNotFoundException()
    {
    }

    public DepartmentNotFoundException(string message)
        : base(message)
    {
    }

    public DepartmentNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}