using wolds_hr_api.Endpoints;
using wolds_hr_api.Helper.Extensions;
using wolds_hr_api.Helpers.Converters;

var builder = WebApplication.CreateBuilder(args);

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

EndpointsEmployee.ConfigureRoutes(app);
EndpointsDepartment.ConfigureRoutes(app);

app.Run();