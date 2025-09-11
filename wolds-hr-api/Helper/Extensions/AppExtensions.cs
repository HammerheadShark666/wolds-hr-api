using Asp.Versioning;
using Asp.Versioning.Builder;
using wolds_hr_api.Data.Context;

namespace wolds_hr_api.Helper.Extensions;

public static class AppExtensions
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

    public static ApiVersionSet GetVersionSet(this WebApplication webApplication)
    {
        return webApplication.NewApiVersionSet()
                             .HasApiVersion(new ApiVersion(1))
                             .ReportApiVersions()
                             .Build();
    }
}