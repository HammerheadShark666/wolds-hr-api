using employee_test_api.Domain;
using employee_test_api.Helper.Dto.Responses;
using employee_test_api.Helpers.Dto.Responses;
using employee_test_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;

namespace employee_test_api.Endpoints;

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
            {
                return Results.NotFound();
            }
            return Results.Ok(employee);
        })
       .WithName("GetEmployee")
       .WithOpenApi(x => new OpenApiOperation(x)
       {
           Summary = "Get employee",
           Description = "Gets employee",
           Tags = [new() { Name = "HR System" }]
       });


        employeeGroup.MapPost("/add", async (IEmployeeService employeeService, Employee employee) =>
        {
            var (isValid, savedEmployee, errors) = await employeeService.Add(employee);
            if (!isValid)
            {
                return Results.BadRequest(new FailedValidationResponse { Errors = errors != null ? errors : [] });
            }

            return Results.Ok(employee);

        })
        .Accepts<Employee>("application/json")
        .Produces<Employee>((int)HttpStatusCode.OK)
        .Produces<FailedValidationResponse>((int)HttpStatusCode.BadRequest)
        .WithName("AddEmployee")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Add employee",
            Description = "Add employee",
            Tags = [new() { Name = "HR System" }]
        });

        employeeGroup.MapPut("/update", async (IEmployeeService employeeService, Employee employee) =>
        {
            var (isValid, savedEmployee, errors) = await employeeService.Update(employee); ;
            if (!isValid)
            {
                return Results.BadRequest(new FailedValidationResponse { Errors = errors != null ? errors : [] });
            }

            return Results.Ok(savedEmployee);

        })
        .Accepts<Employee>("application/json")
        .Produces<Employee>((int)HttpStatusCode.OK)
        .Produces<FailedValidationResponse>((int)HttpStatusCode.BadRequest)
        .WithName("UpdateEmployee")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Update employee",
            Description = "Update employee",
            Tags = [new() { Name = "HR System" }]
        });

        employeeGroup.MapDelete("/{id}", (IEmployeeService employeeService, int id) =>
        {
            return employeeService.Delete(id);
        })
        .WithName("DeleteEmployees")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Delete employee",
            Description = "Delete employee",
            Tags = [new() { Name = "HR System" }]
        });
    }
}