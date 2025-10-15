using Asp.Versioning;
using wolds_hr_api.Endpoint;
using wolds_hr_api.Library.ExceptionHandlers;
using wolds_hr_api.Library.Extensions;
using wolds_hr_api.Library.Helpers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var environmentHelper = new EnvironmentHelper();
builder.Services.AddSingleton<IEnvironmentHelper>(environmentHelper);

builder.Services.ConfigureProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.ConfigureJWT(environmentHelper);
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.BuildCorsPolicy(environmentHelper);
builder.Services.ConfigureJsonSerializer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureDI(environmentHelper);
builder.Services.ConfigureApiVersioning();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.BuildDatabase();
app.UseExceptionHandler();

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

EndpointsAuthentication.ConfigureRoutes(app, versionSet);
EndpointsRefreshToken.ConfigureRoutes(app, versionSet);
EndpointsEmployee.ConfigureRoutes(app, versionSet);
EndpointsDepartment.ConfigureRoutes(app, versionSet);
EndpointsImportEmployee.ConfigureRoutes(app, versionSet);
EndpointsImportEmployeeHistory.ConfigureRoutes(app, versionSet);

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedDatabaseAsync();
}

app.Run();