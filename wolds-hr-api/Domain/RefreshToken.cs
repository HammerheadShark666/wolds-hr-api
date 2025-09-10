using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wolds_hr_api.Domain;

[Table("WOLDS_HR_RefreshToken")]
public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }
    public Account Account { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.Now >= Expires;
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;
    public DateTime? Revoked { get; set; }
    public string RevokedByIp { get; set; } = string.Empty;
    public string ReplacedByToken { get; set; } = string.Empty;
    public bool IsActive => Revoked == null && !IsExpired;
}