using Asp.Versioning;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Endpoint;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Extensions;
using wolds_hr_api.Helpers.Converters;
using wolds_hr_api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureJWT();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.BuildCorsPolicy();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

builder.Services.ConfigureSwagger();
builder.Services.ConfigureDI();
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
app.UseMiddleware<ErrorHandlingMiddleware>();

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

EndpointsAuthentication.ConfigureRoutes(app, versionSet);
EndpointsEmployee.ConfigureRoutes(app, versionSet);
EndpointsDepartment.ConfigureRoutes(app, versionSet);
EndpointsImportEmployee.ConfigureRoutes(app, versionSet);
EndpointsImportEmployeeHistory.ConfigureRoutes(app, versionSet);

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WoldsHrDbContext>();
    await DataSeeder.SeedDatabaseAsync(context);
}

app.Run();