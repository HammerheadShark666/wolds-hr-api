namespace Wolds.Hr.Api.Library.Dto.Responses;

public class ImportEmployeeHistoryLatestResponse
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int ImportedEmployeesCount { get; set; }
    public int ImportedEmployeesErrorsCount { get; set; }
    public int ImportedEmployeesExistingCount { get; set; }
}