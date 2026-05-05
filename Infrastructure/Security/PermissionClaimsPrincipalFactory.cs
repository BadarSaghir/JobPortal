using System.Security.Claims;
using Career635.Domain.Entities.Auth;
using Career635.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Career635.Infrastructure.Security;

public class PermissionClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
{
    private readonly AppDbContext _context;

    public PermissionClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor,
        AppDbContext context)
        : base(userManager, roleManager, optionsAccessor)
    {
        _context = context;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        // 1. Generate the base identity (includes Name, Email, and Roles)
        var identity = await base.GenerateClaimsAsync(user);

        // 2. Get all Role IDs the user belongs to
        var userRoles = await UserManager.GetRolesAsync(user);
        var roleIds = await _context.Roles
            .Where(r => userRoles.Contains(r.Name!))
            .Select(r => r.Id)
            .ToListAsync();

        // 3. Fetch granular permissions from the junction table
        // We filter by IsDeleted to respect your Soft Delete requirement
        var permissions = await _context.RolePermissions
            .Include(rp => rp.Permission)
            .Where(rp => roleIds.Contains(rp.RoleId) && !rp.Permission.IsDeleted)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToListAsync();

        // 4. Add each permission as a "Permission" claim
        foreach (var permission in permissions)
        {
            identity.AddClaim(new Claim("Permission", permission));
        }

        // 5. Add custom bio data as claims (Optional, useful for UI headers)
        if (!string.IsNullOrEmpty(user.FullName))
            identity.AddClaim(new Claim("FullName", user.FullName));

        return identity;
    }
}