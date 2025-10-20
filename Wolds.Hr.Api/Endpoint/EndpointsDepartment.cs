using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Domain;
using wolds_hr_api.Library.Dto.Requests.Department;
using wolds_hr_api.Library.Dto.Responses;
using wolds_hr_api.Library.Exceptions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsDepartment
{
    public static void ConfigureRoutes(this WebApplication webApplication, ApiVersionSet versionSet)
    {
        var departmentGroup = webApplication.MapGroup("v{version:apiVersion}/departments")
                                            .WithTags("v{version:apiVersion}/departments")
                                            .WithApiVersionSet(versionSet)
                                            .MapToApiVersion(1.0);

        departmentGroup.MapGet("", async ([FromServices] IDepartmentService departmentService) =>
        {
            var departments = await departmentService.GetAsync();
            return Results.Ok(departments);
        })
        .RequireAuthorization()
        .Produces<DepartmentResponse>((int)HttpStatusCode.OK)
        .WithName("GetDepartments")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get departments",
            Description = "Gets departments",
            Tags = [new() { Name = "Wolds HR - Department" }]
        });

        departmentGroup.MapGet("/department/{id}", async (Guid id, [FromServices] IDepartmentService departmentService) =>
        {
            var department = await departmentService.GetAsync(id);
            if (department == null)
                return Results.NotFound();

            return Results.Ok(department);
        })
        .WithName("GetDepartmentById")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get department",
            Description = "Gets department",
            Tags = [new() { Name = "Wolds HR - Department" }]
        });

        departmentGroup.MapGet("/department/name/{name}", async (string name, [FromServices] IDepartmentService departmentService) =>
        {
            var department = await departmentService.GetAsync(name);
            if (department == null)
                return Results.NotFound();

            return Results.Ok(department);
        })
        .WithName("GetDepartmentByName")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get department",
            Description = "Gets department",
            Tags = [new() { Name = "Wolds HR - Department" }]
        });

        departmentGroup.MapPost("", async (AddDepartmentRequest addDepartmentRequest, [FromServices] IDepartmentService departmentService) =>
        {
            var (isValid, savedDepartment, errors) = await departmentService.AddAsync(addDepartmentRequest);
            if (!isValid)
                return Results.BadRequest(new FailedValidationResponse { Errors = errors ?? ([]) });

            return Results.Ok(savedDepartment);

        })
        .Accepts<Department>("application/json")
        .Produces<Department>((int)HttpStatusCode.OK)
        .Produces<FailedValidationResponse>((int)HttpStatusCode.BadRequest)
        .WithName("AddDepartment")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Add department",
            Description = "Add department",
            Tags = [new() { Name = "Wolds HR - Department" }]
        });

        departmentGroup.MapPut("", async (UpdateDepartmentRequest updateDepartmentRequest, [FromServices] IDepartmentService departmentService) =>
        {
            var (isValid, savedDepartment, errors) = await departmentService.UpdateAsync(updateDepartmentRequest); ;
            if (!isValid)
                return Results.BadRequest(new FailedValidationResponse { Errors = errors ?? ([]) });

            return Results.Ok(savedDepartment);

        })
        .Accepts<Department>("application/json")
        .Produces<Department>((int)HttpStatusCode.OK)
        .Produces<FailedValidationResponse>((int)HttpStatusCode.BadRequest)
        .WithName("UpdateDepartment")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Update department",
            Description = "Update department",
            Tags = [new() { Name = "Wolds HR - Department" }]
        });

        departmentGroup.MapDelete("/{id}", async (Guid id, [FromServices] IDepartmentService departmentService) =>
        {
            try
            {
                await departmentService.DeleteAsync(id);
                return Results.Ok();
            }
            catch (DepartmentNotFoundException)
            {
                return Results.NotFound(new { Message = "Department not found." });
            }
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("DeleteDepartments")
        .RequireAuthorization()
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Delete department",
            Description = "Delete department",
            Tags = [new() { Name = "Wolds HR - Department" }]
        });
    }
}