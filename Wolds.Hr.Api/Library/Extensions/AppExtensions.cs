using wolds_hr_api.Data.Context;

namespace wolds_hr_api.Library.Extensions;

internal static class AppExtensions
{
    public static void AddCors(this WebApplication webApplication)
    {
        webApplication.UseCors("WoldsHrFrontendPolicy");
    }

    public static void BuildDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WoldsHrDbContext>();
        db.Database.EnsureCreated();
    }
}