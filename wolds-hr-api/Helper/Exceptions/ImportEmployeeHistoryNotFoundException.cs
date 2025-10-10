namespace wolds_hr_api.Helper.Exceptions;

internal sealed class ImportEmployeeHistoryNotFoundException : Exception
{
    public ImportEmployeeHistoryNotFoundException() : base(ConstantMessages.ImportEmployeeNotFound) { }
}
