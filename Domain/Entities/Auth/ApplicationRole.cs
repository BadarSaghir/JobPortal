using Microsoft.AspNetCore.Identity;
using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class ApplicationRole : IdentityRole<Guid>, ISoftDeletable
{
    public string? Description { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation to Permissions
    public virtual ICollection<ApplicationRolePermission> RolePermissions { get; set; } = new List<ApplicationRolePermission>();

    public ApplicationRole() : base() { Id = Guid.NewGuid(); }
    public ApplicationRole(string roleName) : base(roleName) { Id = Guid.NewGuid(); }
}