using System.ComponentModel.DataAnnotations.Schema;

namespace wolds_hr_api.Domain;

[Table("WOLDS_HR_Department")]
public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
