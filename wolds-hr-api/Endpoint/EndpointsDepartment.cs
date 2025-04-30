using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using wolds_hr_api.Helpers.Dto.Responses;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Endpoint;

public static class EndpointsDepartment
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var departmentGroup = webApplication.MapGroup("departments").WithTags("departments");

        departmentGroup.MapGet("", ([FromServices] IDepartmentService departmentService) =>
        {
            var departments = departmentService.Get();
            return Results.Ok(departments);
        })
       .Produces<DepartmentResponse>((int)HttpStatusCode.OK)
       .WithName("GetDepartments")
       .WithOpenApi(x => new OpenApiOperation(x)
       {
           Summary = "Get departments",
           Description = "Gets departments",
           Tags = [new() { Name = "HR System" }]
       });
    }
}