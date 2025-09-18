using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Extensions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsImportEmployee
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var importEmployeeGroup = webApplication.MapGroup("v{version:apiVersion}/import-employees/").WithTags("import-employees");

        importEmployeeGroup.MapPost("", async (HttpRequest request, [FromServices] IImportEmployeeService importEmployeeService) =>
        {
            if (!request.HasFormContentType)
                return Results.BadRequest(new { Message = "Invalid content type." });

            var form = await request.ReadFormAsync();
            var file = form.Files.GetFile("file");

            if (file == null || file.Length == 0)
                return Results.BadRequest(new { Message = "No file uploaded." });

            var fileLines = await importEmployeeService.ReadAllLinesAsync(file);
            if (fileLines.Count == 0)
                return Results.BadRequest(new { Message = "File is empty." });


            if (await importEmployeeService.MaximumNumberOfEmployeesReachedAsync(fileLines))
                return Results.BadRequest(new { Message = $"Maximum number of employees reached: {Constants.MaxNumberOfEmployees}" });

            return Results.Ok(await importEmployeeService.ImportAsync(fileLines)); ;
        })
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<ImportEmployeeHistorySummaryResponse>((int)HttpStatusCode.OK)
        .WithName("ImportEmployees")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Import employees",
            Description = "Import employees",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });
    }
}
