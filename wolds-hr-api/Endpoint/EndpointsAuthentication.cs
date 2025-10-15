using Asp.Versioning.Builder;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Library;
using wolds_hr_api.Library.Dto.Requests;
using wolds_hr_api.Library.Dto.Responses;
using wolds_hr_api.Library.Helpers.Interfaces;
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

        authenticateGroup.MapPost("/login", async (HttpContext http, LoginRequest loginRequest, IAuthenticateService authenticateService, ICookieHelper cookieHelper) =>
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

            cookieHelper.SetAccessTokenCookie(http, token);
            cookieHelper.SetRefreshTokenCookie(http, refreshToken);

            return Results.Ok(new { message = "Logged in" });
        })
        .WithName("Login")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Login and return a jwt token and refresh token",
            Description = "Login and return a jwt token and refresh token",
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

    private static void SetDeleteCookie(HttpContext http, string cookieName)
    {
        http.Response.Cookies.Delete(cookieName, new CookieOptions { Secure = true, SameSite = SameSiteMode.None });
    }
}