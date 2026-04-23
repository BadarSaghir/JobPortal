using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class ApplicationPermission : ISoftDeletable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty; // Code name: jobs.manage
    public string DisplayName { get; set; } = string.Empty; // Human name: Manage Job Postings
    public string Module { get; set; } = string.Empty; // Grouping: Jobs, Auth, Applicants

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual ICollection<ApplicationRolePermission> RolePermissions { get; set; } = new List<ApplicationRolePermission>();
}