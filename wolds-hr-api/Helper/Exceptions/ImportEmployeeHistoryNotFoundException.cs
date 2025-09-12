namespace wolds_hr_api.Helper.Exceptions;

public class ImportEmployeeHistoryNotFoundException : Exception
{
    public ImportEmployeeHistoryNotFoundException()
    {
    }

    public ImportEmployeeHistoryNotFoundException(string message)
        : base(message)
    {
    }

    public ImportEmployeeHistoryNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
