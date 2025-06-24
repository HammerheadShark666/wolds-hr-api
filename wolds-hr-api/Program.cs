using wolds_hr_api.Endpoint;
using wolds_hr_api.Helper.Extensions;
using wolds_hr_api.Helpers.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureJWT();
builder.Services.ConfigureDbContext();
builder.Services.AddEndpointsApiExplorer();
builder.Services.BuildCorsPolicy();
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

app.AddCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.BuildDatabase();

EndpointsAuthentication.ConfigureRoutes(app);
EndpointsEmployee.ConfigureRoutes(app);
EndpointsDepartment.ConfigureRoutes(app);
EndpointsEmployeeImport.ConfigureRoutes(app);

app.Run();