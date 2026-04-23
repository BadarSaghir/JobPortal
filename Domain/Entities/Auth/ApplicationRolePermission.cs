using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class ApplicationRolePermission : ISoftDeletable
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual ApplicationRole Role { get; set; } = null!;
    public virtual ApplicationPermission Permission { get; set; } = null!;
}