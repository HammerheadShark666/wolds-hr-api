using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Extensions;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsDepartment
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var departmentGroup = webApplication.MapGroup("v{version:apiVersion}/departments/").WithTags("departments");

        departmentGroup.MapGet("", ([FromServices] IDepartmentService departmentService) =>
        {
            var departments = departmentService.Get();
            return Results.Ok(departments);
        })
       .RequireAuthorization()
       .Produces<DepartmentResponse>((int)HttpStatusCode.OK)
       .WithName("GetDepartments")
       .WithApiVersionSet(webApplication.GetVersionSet())
       .MapToApiVersion(new ApiVersion(1, 0))
       .WithOpenApi(x => new OpenApiOperation(x)
       {
           Summary = "Get departments",
           Description = "Gets departments",
           Tags = [new() { Name = "Wolds HR - Department" }]
       });
    }
}