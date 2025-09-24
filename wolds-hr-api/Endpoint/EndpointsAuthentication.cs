using Asp.Versioning.Builder;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Requests;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsAuthentication
{
    public static void ConfigureRoutes(this WebApplication webApplication, ApiVersionSet versionSet)
    {
        var authenticateGroup = webApplication.MapGroup("v{version:apiVersion}/")
                                              .WithTags("authenticate")
                                              .WithApiVersionSet(versionSet)
                                              .MapToApiVersion(1.0);

        authenticateGroup.MapPost("/login", async (HttpContext http, LoginRequest loginRequest, IAuthenticateService authenticateService) =>
        {
            http.Response.Headers.CacheControl = "no-store"; // Disable caching
            http.Response.Headers.Pragma = "no-cache";

            var (isValid, authenticated, errors) = await authenticateService.AuthenticateAsync(loginRequest, "ipAddress");
            if (!isValid)
                return Results.BadRequest(new FailedValidationResponse { Errors = errors ?? ([]) });

            if (authenticated == null)
                return Results.BadRequest(new FailedValidationResponse { Errors = errors ?? (["Error logging in."]) });

            var token = authenticated.Token;
            var refreshToken = authenticated.RefreshToken;

            SetAccessTokenCookie(http, token);
            SetRefreshTokenCookie(http, refreshToken);

            return Results.Ok(new { message = "Logged in" });
        })
        .WithName("Login")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Login and return a jwt token and refresh token",
            Description = "Login and return a jwt token and refresh token",
            Tags = [new() { Name = "Wolds HR - Authenticate" }]
        });

        authenticateGroup.MapPost("/refresh-token", async (HttpContext http, JwtRefreshTokenRequest jwtRefreshTokenRequest, IAuthenticateService authenticateService, HttpContext context) =>
        {
            http.Response.Headers.CacheControl = "no-store"; // Disable caching
            http.Response.Headers.Pragma = "no-cache";

            try
            {
                var refreshToken = http.Request.Cookies[Constants.RefreshToken];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Results.BadRequest("Refresh token invalid.");
                }
                var tokens = await authenticateService.RefreshTokenAsync(refreshToken, JWTHelper.IpAddress(context));

                SetAccessTokenCookie(http, tokens.Token);
                SetRefreshTokenCookie(http, tokens.RefreshToken);

                return Results.Ok(new { message = "Refresh token created" });
            }
            catch (RefreshTokenNotFoundException)
            {
                return Results.BadRequest("Refresh token invalid.");
            }
        })
        .Accepts<JwtRefreshTokenRequest>("application/json")
        .Produces<JwtRefreshToken>((int)HttpStatusCode.OK)
        .Produces<FailedValidationResponse>((int)HttpStatusCode.BadRequest)
        .WithName("RefreshToken")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Authenticate refresh token and return a new jwt token and refresh token",
            Description = "Authenticate refresh token and return a new jwt token and refresh token",
            Tags = [new() { Name = "Wolds HR - Authenticate" }]
        });

        authenticateGroup.MapPost("/logout", async (HttpContext http, IRefreshTokenService refreshTokeService) =>
        {
            var refreshToken = http.Request.Cookies[Constants.RefreshToken];
            if (refreshToken != null)
            {
                await refreshTokeService.DeleteRefreshTokenAsync(refreshToken);
            }

            SetDeleteCookie(http, Constants.AccessToken);
            SetDeleteCookie(http, Constants.RefreshToken);

            return Results.Ok();
        })
        .Produces<JwtRefreshToken>((int)HttpStatusCode.OK)
        .WithName("Logout")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Logout of api",
            Description = "Logout of api",
            Tags = [new() { Name = "Wolds HR - Authenticate" }]
        });

        authenticateGroup.MapGet("/authentication/me", (HttpContext http) =>
        {
            http.Response.Headers.CacheControl = "no-store"; // Disable caching
            http.Response.Headers.Pragma = "no-cache";

            return Results.Ok(http.User.Claims.Select(c => new { c.Type, c.Value }));
        })
        .RequireAuthorization()
        .WithName("Authenticate")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Authenticate token",
            Description = "Authenticate token",
            Tags = [new() { Name = "Wolds HR - Authenticate" }]
        });
    }

    private static void SetAccessTokenCookie(HttpContext http, string token)
    {
        if (EnvironmentVariablesHelper.HostDomain == Constants.LocalHost)
        {
            http.Response.Cookies.Append(Constants.AccessToken, token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(EnvironmentVariablesHelper.JWTSettingsTokenExpiryMinutes)
            });
        }
        else
        {
            http.Response.Cookies.Append(Constants.AccessToken, token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Domain = EnvironmentVariablesHelper.HostDomain,
                Expires = DateTimeOffset.UtcNow.AddMinutes(EnvironmentVariablesHelper.JWTSettingsTokenExpiryMinutes)
            });
        }
    }

    private static void SetRefreshTokenCookie(HttpContext http, string refreshToken)
    {
        if (EnvironmentVariablesHelper.HostDomain == Constants.LocalHost)
        {
            http.Response.Cookies.Append(Constants.RefreshToken, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddDays(EnvironmentVariablesHelper.JWTSettingsRefreshTokenExpiryDays)
            });
        }
        else
        {
            http.Response.Cookies.Append(Constants.RefreshToken, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Domain = EnvironmentVariablesHelper.HostDomain,
                Expires = DateTimeOffset.UtcNow.AddDays(EnvironmentVariablesHelper.JWTSettingsRefreshTokenExpiryDays)
            });
        }

    }

    private static void SetDeleteCookie(HttpContext http, string cookieName)
    {
        http.Response.Cookies.Delete(cookieName, new CookieOptions { Secure = true, SameSite = SameSiteMode.None });
    }
}