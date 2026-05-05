using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class PayScale : ISoftDeletable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Grade { get; set; } = string.Empty; // e.g., BPS-17
    public string? Description { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}