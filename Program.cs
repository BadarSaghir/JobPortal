using Career635.Domain.Constants;
using Career635.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache(); // Required for Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Give users 1 hour to fill the 8 pages
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Standard MVC with Views
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthorization(options =>
    {
        // Dynamically create a policy for every permission defined in our constants
        foreach (var permission in AppPermissions.AllPermissions)
        {
            options.AddPolicy(permission.Name, policy => 
                policy.RequireClaim("Permission", permission.Name));
        }
        
        // Special Policy for SuperAdmins to bypass all checks
        options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
    });
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DatabaseSeeder.SeedAllAsync(services);
        Console.WriteLine("Database Seeded Successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during seeding: {ex.Message}");
    }
}

app.UseStaticFiles(); // Essential for local dist/style.css and lib/lucide.min.js
app.UseRouting();
app.UseSession(); // MUST be after UseRouting and before MapControllerRoute
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
    name: "admin_route",
    areaName: "Admin",
    pattern: "{Admin}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "candidate_route",
    areaName: "Candidate",
    pattern: "{controller=Wizard}/{action=apply}/{id?}");

// 2. Default Public Route


app.Run();