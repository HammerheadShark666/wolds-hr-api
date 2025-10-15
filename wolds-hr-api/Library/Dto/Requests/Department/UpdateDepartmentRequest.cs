namespace wolds_hr_api.Library.Dto.Requests.Department;

public class UpdateDepartmentRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
