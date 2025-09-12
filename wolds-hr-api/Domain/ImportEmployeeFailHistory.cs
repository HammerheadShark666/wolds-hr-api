using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wolds_hr_api.Domain;

public class ImportEmployeeFailHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Employee { get; set; } = string.Empty;

    public Guid ImportEmployeeHistoryId { get; set; }

    public required List<ImportEmployeeFailErrorHistory> Errors { get; set; } = [];
}