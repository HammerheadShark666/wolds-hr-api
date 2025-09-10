using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wolds_hr_api.Domain;

[Table("WOLDS_HR_EmployeeImport")]
public class EmployeeImport
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public required ICollection<Employee> Employees { get; set; }

    public required ICollection<ExistingEmployee> ExistingEmployees { get; set; }
}