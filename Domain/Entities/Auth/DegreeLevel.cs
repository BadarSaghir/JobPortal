using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class DegreeLevel : ISoftDeletable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty; // e.g., Bachelors, PhD
    public int LevelOrder { get; set; } // For sorting (e.g., PhD = 1, Matric = 5)

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}