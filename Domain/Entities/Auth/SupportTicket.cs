using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class SupportTicket : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = "Open"; // Open, Resolved, Ignored
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
}