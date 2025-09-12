using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wolds_hr_api.Domain;

public class ImportEmployeeFailErrorHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Error { get; set; } = string.Empty;

    public Guid ImportEmployeeFailHistoryId { get; set; }

    public ImportEmployeeFailHistory ImportEmployeeFailHistory { get; set; } = null!;
}
