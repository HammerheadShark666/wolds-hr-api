using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsEmployee
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var employeeGroup = webApplication.MapGroup("employees").WithTags("employees");

        employeeGroup.MapGet("/search", (string keyword, int page, int pageSize, [FromServices] IEmployeeService employeeService) =>
        {
            var employees = employeeService.Search(keyword, page, pageSize);
            return Results.Ok(employees);
        })
        .Produces<EmployeePagedResponse>((int)HttpStatusCode.OK)
        .WithName("GetEmployeesWithPaging")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get paged employees",
            Description = "Gets employees by paging",
            Tags = [new() { Name = "HR System" }]
        });

        employeeGroup.MapGet("/employee/{id}", (IEmployeeService employeeService, int id) =>
        {
            var employee = employeeService.Get(id);
            if (employee == null)
                return Results.NotFound();

            return Results.Ok(employee);
        })
       .WithName("GetEmployee")
       .RequireAuthorization()
       .WithOpenApi(x => new OpenApiOperation(x)
       {
           Summary = "Get employee",
           Description = "Gets employee",
           Tags = [new() { Name = "HR System" }]
       });


        employeeGroup.MapPost("/add", async (IEmployeeService employeeService, Employee employee) =>
        {
            var (isValid, savedEmployee, errors) = await employeeService.AddAsync(employee);
            if (!isValid)
                return Results.BadRequest(new FailedValidationResponse { Errors = errors ?? ([]) });

            return Results.Ok(employee);

        })
        .Accepts<Employee>("application/json")
        .Produces<Employee>((int)HttpStatusCode.OK)
        .Produces<FailedValidationResponse>((int)HttpStatusCode.BadRequest)
        .WithName("AddEmployee")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Add employee",
            Description = "Add employee",
            Tags = [new() { Name = "HR System" }]
        });

        employeeGroup.MapPut("/update", async (IEmployeeService employeeService, Employee employee) =>
        {
            var (isValid, savedEmployee, errors) = await employeeService.UpdateAsync(employee); ;
            if (!isValid)
                return Results.BadRequest(new FailedValidationResponse { Errors = errors ?? ([]) });

            return Results.Ok(savedEmployee);

        })
        .Accepts<Employee>("application/json")
        .Produces<Employee>((int)HttpStatusCode.OK)
        .Produces<FailedValidationResponse>((int)HttpStatusCode.BadRequest)
        .WithName("UpdateEmployee")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Update employee",
            Description = "Update employee",
            Tags = [new() { Name = "HR System" }]
        });

        employeeGroup.MapDelete("/{id}", (IEmployeeService employeeService, int id) =>
        {
            try
            {
                employeeService.Delete(id);
                return Results.Ok();
            }
            catch (EmployeeNotFoundException)
            {
                return Results.NotFound(new { Message = "Employee not found." });
            }
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("DeleteEmployees")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Delete employee",
            Description = "Delete employee",
            Tags = [new() { Name = "HR System" }]
        });

        employeeGroup.MapPost("/upload-photo/{id}", async (int id, HttpRequest request, IEmployeeService employeeService) =>
        {
            if (!request.HasFormContentType)
                return Results.BadRequest(new { Message = "Invalid content type." });

            var form = await request.ReadFormAsync();
            var file = form.Files[0];

            if (file == null || file.Length == 0)
                return Results.BadRequest(new { Message = "No file uploaded." });

            var newFileName = await employeeService.UpdateEmployeePhotoAsync(id, file);

            return Results.Ok(new UpdatedPhotoResponse(id, newFileName)); ;
        })
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<UpdatedPhotoResponse>((int)HttpStatusCode.OK)
        .WithName("UploadPhoto")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Upload employee photo",
            Description = "Upload employee photo",
            Tags = [new() { Name = "HR System" }]
        });

        employeeGroup.MapPost("/import", async (HttpRequest request, IEmployeeService employeeService) =>
        {
            if (!request.HasFormContentType)
                return Results.BadRequest(new { Message = "Invalid content type." });

            var form = await request.ReadFormAsync();
            var file = form.Files.GetFile("file");

            if (file == null || file.Length == 0)
                return Results.BadRequest(new { Message = "No file uploaded." });

            if (employeeService.MaximumNumberOfEmployeesReached(file))
                return Results.BadRequest(new { Message = $"Maximum number of employees reached: {Constants.MaxNumberOfEmployees}" });

            return Results.Ok(await employeeService.ImportAsync(file)); ;
        })
       .Accepts<IFormFile>("multipart/form-data")
       .Produces<EmployeeImportResponse>((int)HttpStatusCode.OK)
       .WithName("ImportEmployees")
       .RequireAuthorization()
       .WithOpenApi(x => new OpenApiOperation(x)
       {
           Summary = "Import employees",
           Description = "Import employees",
           Tags = [new() { Name = "HR System" }]
       });

        employeeGroup.MapGet("/imported", (DateOnly importDate, int page, int pageSize, [FromServices] IEmployeeService employeeService) =>
        {
            var employees = employeeService.GetImported(importDate, page, pageSize);
            return Results.Ok(employees);
        })
        .Produces<EmployeePagedResponse>((int)HttpStatusCode.OK)
        .WithName("GetImportedEmployeesWithPaging")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get paged imported employees",
            Description = "Gets imported employees by paging",
            Tags = [new() { Name = "HR System" }]
        });
    }
}