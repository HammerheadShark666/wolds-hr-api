using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace wolds_hr_api.Data.Context;

internal sealed class WoldsHrDbContextFactory : IDesignTimeDbContextFactory<WoldsHrDbContext>
{
    public WoldsHrDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<WoldsHrDbContext>();

        optionsBuilder.UseSqlServer(config.GetConnectionString("SQLAZURECONNSTR_WOLDS_HR"));

        return new WoldsHrDbContext(optionsBuilder.Options);
    }
}
