using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class Department : ISoftDeletable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; } // e.g., IT-01

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}