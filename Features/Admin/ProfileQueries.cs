using Career635.Areas.Admin.Models;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Darker;

public class GetUserProfileQuery : IQuery<UserProfileViewModel?> { 
    public Guid UserId { get; set; } 
}

public class GetUserProfileHandler(AppDbContext context) : QueryHandlerAsync<GetUserProfileQuery, UserProfileViewModel?>
{
    public override async Task<UserProfileViewModel?> ExecuteAsync(GetUserProfileQuery query, CancellationToken ct = default)
    {
        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Designation)
            .Include(u => u.Department)
            .Include(u => u.PayScale)
            .FirstOrDefaultAsync(u => u.Id == query.UserId, ct);

        if (user == null) return null;

        // Get Roles and Permissions
        var roles = await context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Join(context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
            .ToListAsync(ct);

        var permissions = await context.RolePermissions
            .Include(rp => rp.Permission)
            .Join(context.UserRoles.Where(ur => ur.UserId == user.Id), 
                  rp => rp.RoleId, ur => ur.RoleId, (rp, ur) => rp.Permission.DisplayName)
            .Distinct()
            .ToListAsync(ct);

        return new UserProfileViewModel(
            user.FullName, user.Email!, user.UserName!,
            user.Designation?.Title, user.Department?.Name, user.PayScale?.Grade,
            user.CreatedAt.DateTime, roles!, permissions!);
    }
}