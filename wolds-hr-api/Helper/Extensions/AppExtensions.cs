namespace wolds_hr_api.Helper.Extensions;

public static class AppExtensions
{
    public static void ConfigureCors(this WebApplication webApplication)
    {
        webApplication.UseCors(x => x
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("http://localhost:3000", "http://localhost:3001"));
    }
}