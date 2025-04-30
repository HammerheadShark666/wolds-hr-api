using System.ComponentModel.DataAnnotations;

namespace wolds_hr_api.Domain;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }
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