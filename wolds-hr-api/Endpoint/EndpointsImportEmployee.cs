using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Extensions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsImportEmployee
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var employeeImportGroup = webApplication.MapGroup("v{version:apiVersion}/import-employees/").WithTags("import-employees");

        employeeImportGroup.MapPost("", async (HttpRequest request, IImportEmployeeService employeeImportService) =>
        {
            if (!request.HasFormContentType)
                return Results.BadRequest(new { Message = "Invalid content type." });

            var form = await request.ReadFormAsync();
            var file = form.Files.GetFile("file");

            if (file == null || file.Length == 0)
                return Results.BadRequest(new { Message = "No file uploaded." });

            if (await employeeImportService.MaximumNumberOfEmployeesReachedAsync(file))
                return Results.BadRequest(new { Message = $"Maximum number of employees reached: {Constants.MaxNumberOfEmployees}" });

            return Results.Ok(await employeeImportService.ImportAsync(file)); ;
        })
       .Accepts<IFormFile>("multipart/form-data")
       .Produces<ImportEmployeeHistoryResponse>((int)HttpStatusCode.OK)
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

        //employeeImportGroup.MapPost("", async (HttpRequest request, IImportEmployeeService importEmployeeService) =>
        //{
        //    if (!request.HasFormContentType)
        //        return Results.BadRequest(new { Message = "Invalid content type." });

        //    var form = await request.ReadFormAsync();
        //    var file = form.Files.GetFile("file");

        //    if (file == null || file.Length == 0)
        //        return Results.BadRequest(new { Message = "No file uploaded." });

        //    if (await importEmployeeService.MaximumNumberOfEmployeesReachedAsync(file))
        //        return Results.BadRequest(new { Message = $"Maximum number of employees reached: {Constants.MaxNumberOfEmployees}" });

        //    return Results.Ok(await importEmployeeService.ImportAsync(file)); ;
        //})
        //.Accepts<IFormFile>("multipart/form-data")
        //.Produces<ImportEmployeeHistoryResponse>((int)HttpStatusCode.OK)
        //.WithName("ImportEmployees")
        //.WithApiVersionSet(webApplication.GetVersionSet())
        //.MapToApiVersion(new ApiVersion(1, 0))
        //.RequireAuthorization()
        //.WithOpenApi(x => new OpenApiOperation(x)
        //{
        //    Summary = "Import employees",
        //    Description = "Import employees",
        //    Tags = [new() { Name = "Wolds HR - Employee Import" }]
        //});

        employeeImportGroup.MapGet("/employees", async (Guid id, int page, int pageSize, [FromServices] IImportEmployeeHistoryService employeeImportService) =>
        {
            var employees = await employeeImportService.GetImportedEmployeesHistoryAsync(id, page, pageSize);
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
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });

        employeeImportGroup.MapGet("/existing-employees", async (Guid id, int page, int pageSize, [FromServices] IImportEmployeeHistoryService employeeImportService) =>
        {
            var existingEmployees = await employeeImportService.GetImportedExistingEmployeesHistoryAsync(id, page, pageSize);
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
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });

        employeeImportGroup.MapGet("", async ([FromServices] IImportEmployeeHistoryService employeeImportService) =>
        {
            var employeeImports = await employeeImportService.GetAsync();
            return Results.Ok(employeeImports);
        })
       .Produces<List<ImportEmployeeHistoryResponse>>((int)HttpStatusCode.OK)
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

        employeeImportGroup.MapDelete("/{id}", async (IImportEmployeeHistoryService employeeImportService, Guid id) =>
        {
            try
            {
                await employeeImportService.DeleteAsync(id);
                return Results.Ok();
            }
            catch (ImportEmployeeHistoryNotFoundException)
            {
                return Results.NotFound(new { Message = "Employee Import not found." });
            }
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("DeleteEmployeesImport")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Delete employee import",
            Description = "Delete employee import",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });
    }
}
