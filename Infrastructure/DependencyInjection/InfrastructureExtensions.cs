using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Darker.AspNetCore;
using Quartz;
using Mapster;
using MapsterMapper;
using Career635.Infrastructure.Persistence;
using Career635.Domain.Entities.Auth;
using Career635.Features.Jobs; // Needed for Darker Assembly Scanning
using  Career635.Infrastructure.Security;

namespace Career635.Infrastructure.DependencyInjection;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // 1. CONDITIONAL DATABASE SETUP (MSSQL sa user on Linux / Postgres)
        var provider = config.GetValue<string>("DatabaseProvider") ?? "MSSQL";
        var connectionString = config.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
        {
            if (provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
            {
                options.UseNpgsql(connectionString, 
                    x => x.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
                       .UseSnakeCaseNamingConvention();
            }
            else
            {
                options.UseSqlServer(connectionString, 
                    x => {
                        x.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                        x.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    });
            }
        });

        // 2. CUSTOM IDENTITY SETUP (GUIDs & Master Data Support)
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        
        .AddDefaultTokenProviders().AddClaimsPrincipalFactory<PermissionClaimsPrincipalFactory>(); 

        // Cookie configuration for MVC
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.Cookie.Name = "Career635.Auth";
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
        });

        // 3. BRIGHTER (COMMANDS)
        // Scans the entire project for Command Handlers
        services.AddBrighter(options =>
        {
            options.HandlerLifetime = ServiceLifetime.Scoped;
            // options.commm = ServiceLifetime.Scoped;
        })
        .AutoFromAssemblies();

        // 4. DARKER (QUERIES)
        // Scans for Query Handlers (Essential for Home Page)
        services.AddDarker(options =>
        {
            options.QueryProcessorLifetime = ServiceLifetime.Scoped;
        })
        .AddHandlersFromAssemblies(typeof(Program).Assembly);

        // 5. MAPSTER (OBJECT MAPPING)
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(typeof(Program).Assembly);
        services.AddSingleton(typeAdapterConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        // 6. QUARTZ (BACKGROUND JOBS)
        services.AddQuartz(q =>
        {
        });
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        services.AddScoped<IFileStorageService, FileStorageService>();

        return services;
    }
}