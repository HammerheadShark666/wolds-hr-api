using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wolds_hr_api.Domain;

public class ImportEmployeeHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public required List<Employee> ImportedEmployees { get; set; } = [];

    public required List<ImportEmployeeExistingHistory> ExistingEmployees { get; set; } = [];

    public required List<ImportEmployeeFailedHistory> FailedEmployees { get; set; } = [];
}