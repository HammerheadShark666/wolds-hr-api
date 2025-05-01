using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Interfaces;

namespace wolds_hr_api.Helper;

public class JwtHelper(IConfiguration configuration) : IJwtHelper
{
    private const int ExpirationMinutes = 60;

    public string GenerateJwtToken(Account account)
    {
        var nowUtc = DateTime.UtcNow;
        var expirationTimeUtc = GetExpirationTimeUtc(nowUtc);

        var token = new JwtSecurityToken(issuer: EnvironmentVariablesHelper.JwtIssuer,
                                         claims: GetClaims(account, nowUtc, expirationTimeUtc),
                                         expires: expirationTimeUtc,
                                         signingCredentials: GetSigningCredentials());

        return GetTokenString(token);
    }

    private static string GetTokenString(JwtSecurityToken token)
    {
        return (new JwtSecurityTokenHandler()).WriteToken(token);
    }

    private DateTime GetExpirationTimeUtc(DateTime nowUtc)
    {
        var expirationDuration = TimeSpan.FromMinutes(GetExpirationMinutes());
        return nowUtc.Add(expirationDuration);
    }

    private int GetExpirationMinutes()
    {
        var expirationMinutes = configuration["JwtToken:ExpirationMinutes"];

        if (expirationMinutes is null)
        {
            return ExpirationMinutes;
        }
        else
        {
            return Int32.Parse(expirationMinutes);
        }
    }

    private static SigningCredentials GetSigningCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariablesHelper.JwtSymmetricSecurityKey));
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    private static List<Claim> GetClaims(Account account, DateTime nowUtc, DateTime expirationUtc)
    {
        return [
                    new(JwtRegisteredClaimNames.Sub, "Authentication"),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(nowUtc).ToString(), ClaimValueTypes.Integer64),
                    new(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(expirationUtc).ToString(), ClaimValueTypes.Integer64),
                    new(JwtRegisteredClaimNames.Iss, EnvironmentVariablesHelper.JwtIssuer),
                    new(JwtRegisteredClaimNames.Aud, EnvironmentVariablesHelper.JwtAudience),
                    new(ClaimTypes.NameIdentifier, account.Id.ToString())
               ];
    }
}