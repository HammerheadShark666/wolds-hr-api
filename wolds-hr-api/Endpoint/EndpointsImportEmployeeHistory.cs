using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Extensions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsImportEmployeeHistory
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var importEmployeeGroup = webApplication.MapGroup("v{version:apiVersion}/import-employee-history/").WithTags("import-employee-history");

        importEmployeeGroup.MapGet("/employees", async (Guid id, int page, int pageSize, [FromServices] IImportEmployeeHistoryService importEmployeeHistoryService) =>
        {
            var employees = await importEmployeeHistoryService.GetImportedEmployeesHistoryAsync(id, page, pageSize);
            return Results.Ok(employees);
        })
        .Produces<List<ImportEmployeeHistorySummaryResponse>>((int)HttpStatusCode.OK)
        .WithName("GetImportedEmployeeHistoryWithPaging")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get paged imported employees",
            Description = "Gets imported employees by paging",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });

        importEmployeeGroup.MapGet("/existing-employees", async (Guid id, int page, int pageSize, [FromServices] IImportEmployeeHistoryService importEmployeeHistoryService) =>
        {
            var existingEmployees = await importEmployeeHistoryService.GetImportedEmployeeExistingHistoryAsync(id, page, pageSize);
            return Results.Ok(existingEmployees);
        })
        .Produces<ImportEmployeeExistingHistoryPagedResponse>((int)HttpStatusCode.OK)
        .WithName("GetImportedEmployeeExistingHistoryWithPaging")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get paged imported existing employees",
            Description = "Gets imported existing employees by paging",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });

        importEmployeeGroup.MapGet("/failed", async (Guid id, int page, int pageSize, [FromServices] IImportEmployeeHistoryService importEmployeeHistoryService) =>
        {
            var failedEmployeeImports = await importEmployeeHistoryService.GetImportedEmployeeFailedHistoryAsync(id, page, pageSize);
            return Results.Ok(failedEmployeeImports);
        })
        .Produces<ImportEmployeeExistingHistoryPagedResponse>((int)HttpStatusCode.OK)
        .WithName("GetImportedEmployeeFailHistoryWithPaging")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get paged imported failed employees",
            Description = "Gets imported failed employees by paging",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });

        importEmployeeGroup.MapGet("", async ([FromServices] IImportEmployeeHistoryService importEmployeeHistoryService) =>
        {
            var employeeImports = await importEmployeeHistoryService.GetAsync();
            return Results.Ok(employeeImports);
        })
        .Produces<List<ImportEmployeeHistorySummaryResponse>>((int)HttpStatusCode.OK)
        .WithName("GetEmployeeImports")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get employee import records",
            Description = "Gets employee import records",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });

        importEmployeeGroup.MapDelete("/{id}", async (IImportEmployeeHistoryService importEmployeeHistoryService, Guid id) =>
        {
            try
            {
                await importEmployeeHistoryService.DeleteAsync(id);
                return Results.Ok();
            }
            catch (ImportEmployeeHistoryNotFoundException)
            {
                return Results.NotFound(new { Message = "Import Employee not found." });
            }
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("DeleteImportEmployeeHistory")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Delete import employee",
            Description = "Delete import employee ",
            Tags = [new() { Name = "Wolds HR - Import Employee" }]
        });
    }
}
