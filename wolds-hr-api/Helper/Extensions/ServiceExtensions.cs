using Asp.Versioning;
using Azure.Storage.Blobs;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using wolds_hr_api.Data;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Data.UnitOfWork;
using wolds_hr_api.Data.UnitOfWork.Interfaces;
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
        services.AddSingleton(new BlobServiceClient(EnvironmentVariablesHelper.AzureStorageConnectionString));
        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IImportEmployeeService, ImportEmployeeService>();
        services.AddScoped<IImportEmployeeHistoryService, ImportEmployeeHistoryService>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IImportEmployeeSuccessHistoryRepository, ImportEmployeeSuccessHistoryRepository>();
        services.AddScoped<IImportEmployeeExistingHistoryRepository, ImportEmployeeExistingHistoryRepository>();
        services.AddScoped<IImportEmployeeFailedHistoryRepository, ImportEmployeeFailedHistoryRepository>();
        services.AddScoped<IImportEmployeeHistoryRepository, ImportEmployeeHistoryRepository>();
        services.AddScoped<IImportEmployeeHistoryUnitOfWork, ImportEmployeeHistoryUnitOfWork>();
        services.AddScoped<IAccountUnitOfWork, AccountUnitOfWork>();
        services.AddScoped<IRefreshTokenUnitOfWork, RefreshTokenUnitOfWork>();
        services.AddScoped<IEmployeeUnitOfWork, EmployeeUnitOfWork>();
        services.AddScoped<IDepartmentUnitOfWork, DepartmentUnitOfWork>();
        services.AddScoped<IAzureStorageBlobHelper, AzureStorageBlobHelper>();
        services.AddScoped<IPhotoHelper, PhotoHelper>();
        services.AddScoped<IJWTHelper, JWTHelper>();
        services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();
    }

    public static void ConfigureDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<WoldsHrDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(Constants.DatabaseConnectionString),
            options => options.EnableRetryOnFailure()
            .MigrationsAssembly(typeof(WoldsHrDbContext).Assembly.FullName)));
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

    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}