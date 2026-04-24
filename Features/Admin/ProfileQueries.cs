using Paramore.Darker;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Career635.Infrastructure.Persistence;
using Career635.Areas.Admin.Models;
using Career635.Domain.Entities.Auth;

namespace Career635.Features.Admin;

// 1. THE QUERY DEFINITION
public class GetUserProfileQuery : IQuery<UserProfileViewModel?>
{
    public Guid UserId { get; set; }
}

// 2. THE HANDLER (LOGIC)
public class GetUserProfileHandler(
    AppDbContext context, 
    UserManager<ApplicationUser> userManager) 
    : QueryHandlerAsync<GetUserProfileQuery, UserProfileViewModel?>
{
    public override async Task<UserProfileViewModel?> ExecuteAsync(
        GetUserProfileQuery query, 
        CancellationToken ct = default)
    {
        // A. Fetch the User with all Master Data Includes
        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Department)
            .Include(u => u.Designation)
            .Include(u => u.PayScale)
            .FirstOrDefaultAsync(u => u.Id == query.UserId, ct);

        if (user == null) return null;

        // B. Fetch Assigned Roles (Identity standard)
        var roles = await userManager.GetRolesAsync(user);

        // C. Fetch Granular Permissions through the Role-Permission Matrix
        // We join Roles -> RolePermissions -> Permissions
        var roleIds = await context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.RoleId)
            .ToListAsync(ct);

        var permissions = await context.RolePermissions
            .AsNoTracking()
            .Include(rp => rp.Permission)
            .Where(rp => roleIds.Contains(rp.RoleId) && !rp.Permission.IsDeleted)
            .Select(rp => rp.Permission.DisplayName)
            .Distinct()
            .OrderBy(p => p)
            .ToListAsync(ct);

        // D. Check if User has an Authenticator App Key configured
        var hasAuthenticator = !string.IsNullOrEmpty(await userManager.GetAuthenticatorKeyAsync(user));

        // E. Map to the final ViewModel
        return new UserProfileViewModel(
            FullName: user.FullName,
            Email: user.Email ?? "N/A",
            UserName: user.UserName ?? "N/A",
            Designation: user.Designation?.Title,
            Department: user.Department?.Name,
            PayScale: user.PayScale?.Grade,
            CreatedAt: user.CreatedAt.DateTime,
            Roles: roles,
            Permissions: permissions,
            IsTwoFactorEnabled: user.TwoFactorEnabled,
            HasAuthenticator: hasAuthenticator
        );
    }
}