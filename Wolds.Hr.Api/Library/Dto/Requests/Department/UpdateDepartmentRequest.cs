namespace Wolds.Hr.Api.Library.Dto.Requests.Department;

public class UpdateDepartmentRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
