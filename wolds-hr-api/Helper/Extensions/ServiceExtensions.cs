using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using wolds_hr_api.Data;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Service;
using wolds_hr_api.Service.Interfaces;
using wolds_hr_api.Services;
using wolds_hr_api.Services.Interfaces;
using wolds_hr_api.Validator;

namespace wolds_hr_api.Helper.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "WoldHR API",
                Description = "An ASP.NET Core Web API for accessing/managing WoldHR SqlServer DB",
            });
            //option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            //{
            //    Name = "Authorization",
            //    Type = SecuritySchemeType.ApiKey,
            //    Scheme = "Bearer",
            //    BearerFormat = "JWT",
            //    In = ParameterLocation.Header,
            //    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            //});
            //option.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer"
            //            }
            //        },
            //        Array.Empty<string>()
            //    }
            //});
            //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    public static void ConfigureDI(this IServiceCollection services)
    {
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IAzureStorageBlobHelper, AzureStorageBlobHelper>();
        services.AddScoped<IPhotoHelper, PhotoHelper>();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();
    }
}