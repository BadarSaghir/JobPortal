using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class Designation : ISoftDeletable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}