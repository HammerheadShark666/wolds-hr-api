// Ignore Spelling: Jwt

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace wolds_hr_api.Helper.Extensions;

public static class JwtExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(optiones =>
        {
            optiones.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            optiones.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            optiones.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = EnvironmentVariablesHelper.JWTAudience,
                ValidIssuer = EnvironmentVariablesHelper.JWTIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariablesHelper.JWTSymmetricSecurityKey)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            o.MapInboundClaims = false;
        });
    }
}