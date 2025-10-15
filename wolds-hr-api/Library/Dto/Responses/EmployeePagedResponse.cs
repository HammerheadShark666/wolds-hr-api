namespace wolds_hr_api.Library.Dto.Responses;

internal sealed class EmployeePagedResponse
{
    public List<EmployeeResponse> Employees { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalEmployees / PageSize);
}
