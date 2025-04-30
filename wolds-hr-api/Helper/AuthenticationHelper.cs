using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper;
public class AuthenticationHelper
{
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
            Token = AuthenticationHelper.CreateRandomToken(),
            Expires = expires,
            Created = DateTime.Now,
            CreatedByIp = ipAddress
        };
    }

    public static string GenerateJwtToken(Account account, int expiryMinutes, string secret)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                      new Claim("id", account.Id.ToString()),
                      new Claim(JwtRegisteredClaimNames.Email, account.Email),
                      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.Now.AddMinutes(expiryMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}