using wolds_hr_api.Data.Context;
using wolds_hr_api.Endpoint;
using wolds_hr_api.Helper.Extensions;
using wolds_hr_api.Helpers.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication();
builder.Services.ConfigureDbContext();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

builder.Services.ConfigureSwagger();
builder.Services.ConfigureDI();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

EndpointsAuthentication.ConfigureRoutes(app);
EndpointsEmployee.ConfigureRoutes(app);
EndpointsDepartment.ConfigureRoutes(app);

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.EnsureCreated();

app.Run();