namespace wolds_hr_api.Helper.Dto.Requests.Department;

public class UpdateDepartmentRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
