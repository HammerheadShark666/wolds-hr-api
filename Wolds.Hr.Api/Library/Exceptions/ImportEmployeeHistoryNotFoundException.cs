namespace wolds_hr_api.Library.Exceptions;

internal sealed class ImportEmployeeHistoryNotFoundException : Exception
{
    public ImportEmployeeHistoryNotFoundException() : base(ConstantMessages.ImportEmployeeNotFound) { }
}
