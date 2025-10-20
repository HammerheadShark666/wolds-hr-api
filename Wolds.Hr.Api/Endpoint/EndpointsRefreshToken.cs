using Asp.Versioning.Builder;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Library;
using wolds_hr_api.Library.Dto.Requests;
using wolds_hr_api.Library.Dto.Responses;
using wolds_hr_api.Library.Exceptions;
using wolds_hr_api.Library.Helpers;
using wolds_hr_api.Library.Helpers.Interfaces;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsRefreshToken
{
    public static void ConfigureRoutes(this WebApplication webApplication, ApiVersionSet versionSet)
    {
        var refreshTokenGroup = webApplication.MapGroup("v{version:apiVersion}/")
                                              .WithTags("refreshTokenGroup")
                                              .WithApiVersionSet(versionSet)
                                              .MapToApiVersion(1.0);

        refreshTokenGroup.MapPost("/refresh-token", async (HttpContext http, JwtRefreshTokenRequest jwtRefreshTokenRequest, IRefreshTokenService refreshTokenService, ICookieHelper cookieHelper, HttpContext context) =>
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
                var tokens = await refreshTokenService.RefreshTokenAsync(refreshToken, JWTHelper.IpAddress(context));

                cookieHelper.SetAccessTokenCookie(http, tokens.Token);
                cookieHelper.SetRefreshTokenCookie(http, tokens.RefreshToken);

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
            Tags = [new() { Name = "Wolds HR - Refresh Token" }]
        });
    }
}