namespace Wolds.Hr.Api.Library.Exceptions;

internal sealed class ImportEmployeeHistoryNotFoundException : Exception
{
    public ImportEmployeeHistoryNotFoundException() : base(ConstantMessages.ImportEmployeeNotFound) { }
}
