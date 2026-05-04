using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Career635.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // 1. Manually build configuration to read appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // 2. Determine which provider to use from the settings
        var provider = configuration.GetValue<string>("DatabaseProvider");

        if (provider == "Postgres")
        {
                    var connectionString = configuration.GetConnectionString("PostgresConnection");

            optionsBuilder.UseNpgsql(connectionString)
                          .UseSnakeCaseNamingConvention();
        }
        else
        {
                    var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Default to MSSQL
            optionsBuilder.UseSqlServer(connectionString);
        }

        return new AppDbContext(optionsBuilder.Options);
    }
}