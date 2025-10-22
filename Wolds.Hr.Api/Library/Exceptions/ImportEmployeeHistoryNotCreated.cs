namespace Wolds.Hr.Api.Library.Exceptions;

internal sealed class ImportEmployeeHistoryNotCreated : Exception
{
    public ImportEmployeeHistoryNotCreated() : base(ConstantMessages.ImportEmployeeNotCreated) { }
}