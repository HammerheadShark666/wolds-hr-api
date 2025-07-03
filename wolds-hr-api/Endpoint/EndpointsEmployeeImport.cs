using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsEmployeeImport
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var employeeImportGroup = webApplication.MapGroup("employees-import").WithTags("employees-import");

        employeeImportGroup.MapPost("", async (HttpRequest request, IEmployeeImportService employeeImportService) =>
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
        .Produces<EmployeeImportResponse>((int)HttpStatusCode.OK)
        .WithName("ImportEmployees")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Import employees",
            Description = "Import employees",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });

        employeeImportGroup.MapGet("/employees", async (int id, int page, int pageSize, [FromServices] IEmployeeImportService employeeImportService) =>
        {
            var employees = await employeeImportService.GetImportedEmployeesAsync(id, page, pageSize);
            return Results.Ok(employees);
        })
        .Produces<List<EmployeeImportResponse>>((int)HttpStatusCode.OK)
        .WithName("GetImportedEmployeesWithPaging")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get paged imported employees",
            Description = "Gets imported employees by paging",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });

        employeeImportGroup.MapGet("/existing-employees", async (int id, int page, int pageSize, [FromServices] IEmployeeImportService employeeImportService) =>
        {
            var existingEmployees = await employeeImportService.GetExistingEmployeesImportedAsync(id, page, pageSize);
            return Results.Ok(existingEmployees);
        })
        .Produces<ExistingEmployeePagedResponse>((int)HttpStatusCode.OK)
        .WithName("GetImportedExistingEmployeesWithPaging")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get paged imported existing employees",
            Description = "Gets imported existing employees by paging",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });

        employeeImportGroup.MapDelete("/{id}", async (IEmployeeImportService employeeImportService, int id) =>
        {
            try
            {
                await employeeImportService.DeleteAsync(id);
                return Results.Ok();
            }
            catch (EmployeeImportNotFoundException)
            {
                return Results.NotFound(new { Message = "Employee Import not found." });
            }
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("DeleteEmployeesImport")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Delete employee import",
            Description = "Delete employee import",
            Tags = [new() { Name = "Wolds HR - Employee Import" }]
        });
    }
}
