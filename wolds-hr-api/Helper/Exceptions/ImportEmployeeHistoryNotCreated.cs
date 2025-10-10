namespace wolds_hr_api.Helper.Exceptions;

internal sealed class ImportEmployeeHistoryNotCreated : Exception
{
    public ImportEmployeeHistoryNotCreated() : base(ConstantMessages.ImportEmployeeNotCreated) { }
}
