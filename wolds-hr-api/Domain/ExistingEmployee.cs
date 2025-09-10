using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wolds_hr_api.Domain;

[Table("WOLDS_HR_ExistingEmployee")]
public class ExistingEmployee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Surname { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public DateOnly Created { get; set; }
    public Guid EmployeeImportId { get; set; }
    public EmployeeImport? EmployeeImport { get; set; }
}