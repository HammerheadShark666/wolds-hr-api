using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wolds.Hr.Api.Domain;

public class ImportEmployeeFailedErrorHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Error { get; set; } = string.Empty;
    public Guid ImportEmployeeFailedHistoryId { get; set; }
}
