using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class UserNotification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ActionUrl { get; set; } // Link to the dossier or job
    public string Type { get; set; } = "Info"; // Info, Success, Warning, Urgent
    public bool IsRead { get; set; } = false;

    public virtual ApplicationUser User { get; set; } = null!;
}