using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Library.Dto.Responses;
using wolds_hr_api.Library.Helpers.Interfaces;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsImportEmployee
{
    public static void ConfigureRoutes(this WebApplication webApplication, ApiVersionSet versionSet)
    {
        var importEmployeeGroup = webApplication.MapGroup("v{version:apiVersion}/import-employees")
                                                  .WithTags("import-employees")
                                                  .WithApiVersionSet(versionSet)
                                                  .MapToApiVersion(1.0);

        importEmployeeGroup.MapPost("", async (HttpRequest request, [FromServices] IImportEmployeeService importEmployeeService, IFileHelper fileHelper) =>
        {
            if (!request.HasFormContentType)
                return Results.BadRequest(new { Message = "Invalid content type." });

            var file = await fileHelper.GetFileAsync(request);
            if (file == null || !fileHelper.FileHasContent(file))
                return Results.BadRequest(new { Message = "No data in file." });

            var result = await importEmployeeService.ImportFromFileAsync(file);

            return Results.Ok(result);


        })
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<ImportEmployeeHistorySummaryResponse>((int)HttpStatusCode.OK)
        .WithName("ImportEmployees")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Import employees",
            Description = "Import employees",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });
    }
}