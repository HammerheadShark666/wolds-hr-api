using FluentValidation;
using FluentValidation.AspNetCore;
using wolds_hr_api.Data;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Endpoints;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Interfaces;
using wolds_hr_api.Helpers.Converters;
using wolds_hr_api.Service;
using wolds_hr_api.Service.Interfaces;
using wolds_hr_api.Services;
using wolds_hr_api.Services.Interfaces;
using wolds_hr_api.Validator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAzureStorageBlobHelper, AzureStorageBlobHelper>();
builder.Services.AddScoped<IPhotoHelper, PhotoHelper>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000", "http://localhost:3001"));

app.UseHttpsRedirection();

EndpointsEmployee.ConfigureRoutes(app);
EndpointsDepartment.ConfigureRoutes(app);

app.Run();