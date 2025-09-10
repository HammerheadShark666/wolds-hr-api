using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static wolds_hr_api.Helper.Enums;

namespace wolds_hr_api.Domain;

public class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; }
    public string VerificationToken { get; set; } = string.Empty;
    public DateTime? Verified { get; set; }
    public bool IsAuthenticated => Verified.HasValue || PasswordReset.HasValue;
    public string ResetToken { get; set; } = string.Empty;
    public DateTime? ResetTokenExpires { get; set; }
    public DateTime? PasswordReset { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = [];
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}