using Microsoft.AspNetCore.Identity;
using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class ApplicationUser : IdentityUser<Guid>, ISoftDeletable
{
    public string FullName { get; set; } = string.Empty;
    
    // Master Data Links
    public Guid? DesignationId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? PayScaleId { get; set; }

    // Audit & Soft Delete
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigations
    public virtual Designation? Designation { get; set; }
    public virtual Department? Department { get; set; }
    public virtual PayScale? PayScale { get; set; }

    public ApplicationUser() : base() { Id = Guid.NewGuid(); }
}