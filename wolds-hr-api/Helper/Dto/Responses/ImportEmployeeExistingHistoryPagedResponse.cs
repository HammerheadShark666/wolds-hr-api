namespace wolds_hr_api.Helper.Dto.Responses;

public class ImportEmployeeExistingHistoryPagedResponse
{
    public List<EmployeeResponse> Employees { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalEmployees / PageSize);
}