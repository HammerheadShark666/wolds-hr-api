using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Exceptions;
using wolds_hr_api.Helper.Extensions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsEmployee
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var employeeGroup = webApplication.MapGroup("v{version:apiVersion}/employees/").WithTags("employees");

        employeeGroup.MapGet("/search", async (string keyword, int page, int pageSize, Guid? departmentId, [FromServices] IEmployeeService employeeService) =>
        {
            var employees = await employeeService.SearchAsync(keyword, departmentId, page, pageSize);
            return Results.Ok(employees);
        })
        .Produces<EmployeePagedResponse>((int)HttpStatusCode.OK)
        .WithName("SearchEmployeeWithPaging")
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Searches employees with paging",
            Description = "Searches employees with paging",
            Tags = [new() { Name = "Wolds HR - Employee" }]
        });

        employeeGroup.MapGet("/employee/{id}", async (IEmployeeService employeeService, Guid id) =>
        {
            var employee = await employeeService.GetAsync(id);
            if (employee == null)
                return Results.NotFound();

            return Results.Ok(employee);
        })
       .WithName("GetEmployee")
       .WithApiVersionSet(webApplication.GetVersionSet())
       .MapToApiVersion(new ApiVersion(1, 0))
       .RequireAuthorization()
       .WithOpenApi(x => new OpenApiOperation(x)
       {
           Summary = "Get employee",
           Description = "Gets employee",
           Tags = [new() { Name = "Wolds HR - Employee" }]
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
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Add employee",
            Description = "Add employee",
            Tags = [new() { Name = "Wolds HR - Employee" }]
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
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Update employee",
            Description = "Update employee",
            Tags = [new() { Name = "Wolds HR - Employee" }]
        });

        employeeGroup.MapDelete("/{id}", async (IEmployeeService employeeService, Guid id) =>
        {
            try
            {
                await employeeService.DeleteAsync(id);
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
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Delete employee",
            Description = "Delete employee",
            Tags = [new() { Name = "Wolds HR - Employee" }]
        });

        employeeGroup.MapPost("/upload-photo/{id}", async (Guid id, HttpRequest request, IEmployeeService employeeService) =>
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
        .WithApiVersionSet(webApplication.GetVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Upload employee photo",
            Description = "Upload employee photo",
            Tags = [new() { Name = "Wolds HR - Employee" }]
        });
    }
}