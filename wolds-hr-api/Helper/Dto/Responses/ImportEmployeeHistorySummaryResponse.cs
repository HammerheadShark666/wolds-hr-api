namespace wolds_hr_api.Helper.Dto.Responses;

public class ImportEmployeeHistorySummaryResponse
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int ImportedEmployeesCount { get; set; }
    public int ImportEmployeesExistingCount { get; set; }
    public int ImportEmployeesErrorsCount { get; set; }
}