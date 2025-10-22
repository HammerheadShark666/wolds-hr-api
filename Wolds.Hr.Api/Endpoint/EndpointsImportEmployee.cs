using Asp.Versioning.Builder;
using Microsoft.OpenApi.Models;
using System.Net;
using Wolds.Hr.Api.Library.Dto.Responses;
using Wolds.Hr.Api.Service.Interfaces;

namespace Wolds.Hr.Api.Endpoint;

public static class EndpointsImportEmployee
{
    public static void ConfigureRoutes(this WebApplication webApplication, ApiVersionSet versionSet)
    {
        var importEmployeeGroup = webApplication.MapGroup("v{version:apiVersion}/import/employees")
                                                  .WithTags("import-employees")
                                                  .WithApiVersionSet(versionSet)
                                                  .MapToApiVersion(1.0);

        importEmployeeGroup.MapPost("", async (IFormFile importFile, IImportEmployeeService importEmployeeService) =>
        {
            if (importFile == null || importFile.Length == 0)
                return Results.BadRequest(new { Message = "No data in import file." });

            var result = await importEmployeeService.ImportFromFileAsync(importFile);

            return Results.Ok(result);
        })
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<ImportEmployeeHistorySummaryResponse>((int)HttpStatusCode.OK)
        .WithName("ImportEmployees")
        .RequireAuthorization()
        .DisableAntiforgery()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Import employees",
            Description = "Import employees",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });
    }
}