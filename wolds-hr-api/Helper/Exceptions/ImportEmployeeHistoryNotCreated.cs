namespace wolds_hr_api.Helper.Exceptions;

public class ImportEmployeeHistoryNotCreated : Exception
{
    public ImportEmployeeHistoryNotCreated()
    {
    }

    public ImportEmployeeHistoryNotCreated(string message)
        : base(message)
    {
    }

    public ImportEmployeeHistoryNotCreated(string message, Exception inner)
        : base(message, inner)
    {
    }
}
