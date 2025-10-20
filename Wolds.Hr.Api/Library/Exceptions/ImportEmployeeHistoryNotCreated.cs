namespace wolds_hr_api.Library.Exceptions;

internal sealed class ImportEmployeeHistoryNotCreated : Exception
{
    public ImportEmployeeHistoryNotCreated() : base(ConstantMessages.ImportEmployeeNotCreated) { }
}