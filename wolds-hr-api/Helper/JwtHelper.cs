using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Interfaces;

namespace wolds_hr_api.Helper;

public class JWTHelper() : IJWTHelper
{
    public string GenerateJWTToken(Account account)
    {
        var nowUtc = DateTime.UtcNow;
        var expirationTimeUtc = GetExpirationTimeUtc(nowUtc);

        var token = new JwtSecurityToken(issuer: EnvironmentVariablesHelper.JWTIssuer,
                                         claims: GetClaims(account, nowUtc, expirationTimeUtc),
                                         expires: expirationTimeUtc,
                                         signingCredentials: GetSigningCredentials());

        return GetTokenString(token);
    }

    private static string GetTokenString(JwtSecurityToken token)
    {
        return (new JwtSecurityTokenHandler()).WriteToken(token);
    }

    private static DateTime GetExpirationTimeUtc(DateTime nowUtc)
    {
        var expirationDuration = TimeSpan.FromMinutes(EnvironmentVariablesHelper.JWTSettingsTokenExpiryMinutes);
        return nowUtc.Add(expirationDuration);
    }

    private static SigningCredentials GetSigningCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariablesHelper.JWTSymmetricSecurityKey));
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    private static List<Claim> GetClaims(Account account, DateTime nowUtc, DateTime expirationUtc)
    {
        return [
                    new(JwtRegisteredClaimNames.Sub, "Authentication"),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(nowUtc).ToString(), ClaimValueTypes.Integer64),
                    new(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(expirationUtc).ToString(), ClaimValueTypes.Integer64),
                    new(JwtRegisteredClaimNames.Iss, EnvironmentVariablesHelper.JWTIssuer),
                    new(JwtRegisteredClaimNames.Aud, EnvironmentVariablesHelper.JWTAudience),
                    new(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new("name", account.Email)
               ];
    }

    public static string CreateRandomToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomNumber = new byte[40];
        rng.GetBytes(randomNumber);
        return CleanToken(randomNumber);
    }

    public static string CleanToken(byte[] randomNumber)
    {
        return Convert.ToBase64String(randomNumber).Replace('+', '-')
                                                   .Replace('/', '_')
                                                   .Replace("=", "4")
                                                   .Replace("?", "G")
                                                   .Replace("/", "X");
    }

    public static RefreshToken GenerateRefreshToken(string ipAddress, DateTime expires)
    {
        return new RefreshToken
        {
            Token = CreateRandomToken(),
            Expires = expires,
            Created = DateTime.Now,
            CreatedByIp = ipAddress
        };
    }

    public static string IpAddress(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out Microsoft.Extensions.Primitives.StringValues value))
        {
            var ip = value.ToString();

            if (!string.IsNullOrEmpty(ip))
            {
                return ip;
            }
            else
            {
                return "No ipAddress";
            }
        }

        string? ipAddress = context.Connection?.RemoteIpAddress?.MapToIPv4().ToString();

        if (!string.IsNullOrEmpty(ipAddress))
        {
            return ipAddress;
        }
        else
        {
            return "No ipAddress";
        }
    }
}