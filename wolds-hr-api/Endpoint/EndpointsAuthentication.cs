using Microsoft.OpenApi.Models;
using SwanSong.Domain.Dto;
using System.Net;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsAuthentication
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var authenticateGroup = webApplication.MapGroup("v1").WithTags("authenticate");

        authenticateGroup.MapPost("/login", async (LoginRequest loginRequest, IAuthenticateService authenticateService) =>
        {
            var (isValid, authenticated, errors) = await authenticateService.AuthenticateAsync(loginRequest); //, AuthenticationHelper.IpAddress(request, httpContext) , HttpRequest request, HttpContext httpContext
            if (!isValid)
                return Results.BadRequest(new FailedValidationResponse { Errors = errors != null ? errors : [] });

            return Results.Ok(authenticated);
        })
        .Accepts<LoginRequest>("application/json")
        .Produces<Authenticated>((int)HttpStatusCode.OK)
        .Produces<FailedValidationResponse>((int)HttpStatusCode.BadRequest)
        .WithName("AuthenticateUser")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Authenticate a user",
            Description = "Authenticates a user and returns a token if valid",
            Tags = [new() { Name = "WoldHR - Authenticate" }]
        });
    }
}