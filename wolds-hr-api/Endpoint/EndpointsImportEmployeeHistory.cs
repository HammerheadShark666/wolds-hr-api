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
        var employeeImportGroup = webApplication.MapGroup("v{version:apiVersion}/import-employees-history/").WithTags("import-employees-history");

        employeeImportGroup.MapGet("", async ([FromServices] IImportEmployeeHistoryService importEmployeeHistoryService) =>
        {
            var employeeImports = await importEmployeeHistoryService.GetAsync();
            return Results.Ok(employeeImports);
        })
        .Produces<List<ImportEmployeeHistoryResponse>>((int)HttpStatusCode.OK)
        .WithName("GetEmployeeImports")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get import employee history records",
            Description = "Gets import employee history records",
            Tags = [new() { Name = "Wolds HR - Import Employee History" }]
        });

        employeeImportGroup.MapGet("/employees", async (Guid id, int page, int pageSize, [FromServices] IImportEmployeeHistoryService importEmployeeHistoryService) =>
        {
            var employees = await importEmployeeHistoryService.GetImportedEmployeesHistoryAsync(id, page, pageSize);
            return Results.Ok(employees);
        })
        .Produces<List<ImportEmployeeHistoryResponse>>((int)HttpStatusCode.OK)
        .WithName("GetImportedEmployeesWithPaging")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get paged imported employees",
            Description = "Gets imported employees by paging",
            Tags = [new() { Name = "Wolds HR - Import Employee History" }]
        });

        employeeImportGroup.MapGet("/existing-employees", async (Guid id, int page, int pageSize, [FromServices] IImportEmployeeHistoryService importEmployeeHistoryService) =>
        {
            var existingEmployees = await importEmployeeHistoryService.GetImportedExistingEmployeesHistoryAsync(id, page, pageSize);
            return Results.Ok(existingEmployees);
        })
        .Produces<ImportEmployeeExistingHistoryPagedResponse>((int)HttpStatusCode.OK)
        .WithName("GetImportedExistingEmployeesWithPaging")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get paged imported existing employees",
            Description = "Gets imported existing employees by paging",
            Tags = [new() { Name = "Wolds HR - Import Employee History" }]
        });

        employeeImportGroup.MapGet("/fail", async (Guid id, int page, int pageSize, [FromServices] IImportEmployeeHistoryService importEmployeeHistoryService) =>
        {
            var failedImports = await importEmployeeHistoryService.GetImportedFailedEmployeesHistoryAsync(id, page, pageSize);
            return Results.Ok(failedImports);
        })
        .Produces<ImportEmployeeExistingHistoryPagedResponse>((int)HttpStatusCode.OK)
        .WithName("GetImportedFailedEmployeesWithPaging")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get paged imported failed employees",
            Description = "Gets imported failed employees by paging",
            Tags = [new() { Name = "Wolds HR - Import Employee History" }]
        });

        employeeImportGroup.MapDelete("/{id}", async (Guid id, [FromServices] IImportEmployeeHistoryService importEmployeeHistoryService) =>
        {
            try
            {
                await importEmployeeHistoryService.DeleteAsync(id);
                return Results.Ok();
            }
            catch (ImportEmployeeHistoryNotFoundException)
            {
                return Results.NotFound(new { Message = "Import employee history not found." });
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
            Summary = "Delete import employee history",
            Description = "Delete import employee history",
            Tags = [new() { Name = "Wolds HR - Import Employee History" }]
        });
    }
}
