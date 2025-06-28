using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using wolds_hr_api.Data;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Service;
using wolds_hr_api.Service.Interfaces;
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
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
        });
    }

    public static void ConfigureJWT(this IServiceCollection services)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = EnvironmentVariablesHelper.JWTIssuer,
                    ValidAudience = EnvironmentVariablesHelper.JWTAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariablesHelper.JWTSymmetricSecurityKey)),
                    NameClaimType = "name",
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.HttpContext.Request.Cookies[Constants.AccessToken];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("JWT Authentication failed:");
                        Console.WriteLine(context.Exception.ToString());
                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void ConfigureDI(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmployeeImportService, EmployeeImportService>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IExistingEmployeeRepository, ExistingEmployeeRepository>();
        services.AddScoped<IEmployeeImportRepository, EmployeeImportRepository>();
        services.AddScoped<IAzureStorageBlobHelper, AzureStorageBlobHelper>();
        services.AddScoped<IPhotoHelper, PhotoHelper>();
        services.AddScoped<IJWTHelper, JWTHelper>();

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();
    }

    public static void ConfigureDbContext(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName: "WoldHrDB"));
    }

    public static void BuildCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("WoldsHrFrontendPolicy", policy =>
            {
                policy.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:3001",
                    "https://mango-plant-076b11e1e.6.azurestaticapps.net",
                    "https://wolds-hr.co.uk",
                    "https://www.wolds-hr.co.uk"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });
    }
}