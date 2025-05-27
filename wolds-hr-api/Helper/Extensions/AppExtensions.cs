using wolds_hr_api.Data.Context;

namespace wolds_hr_api.Helper.Extensions;

public static class AppExtensions
{
    public static void ConfigureCors(this WebApplication webApplication)
    {
        webApplication.UseCors(x => x
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("http://localhost:3000", "http://localhost:3001", "https://mango-plant-076b11e1e.6.azurestaticapps.net"));
    }

    public static void BuildDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }
}