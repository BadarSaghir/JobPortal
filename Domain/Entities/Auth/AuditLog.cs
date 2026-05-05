using Career635.Domain.Common;

namespace Career635.Domain.Entities.Auth;

public class AuditLog : BaseEntity
{
    public string? UserId { get; set; }        // ID of logged-in staff
    public string? UserName { get; set; }      // Email of staff
    public string Action { get; set; } = string.Empty; // e.g. "Login", "UpdateStatus"
    public string EntityName { get; set; } = string.Empty; // e.g. "JobApplication"
    public string? EntityId { get; set; }      // The Guid of the record changed
    
    // Technical Data
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }     // Browser/Device info
    public string? Path { get; set; }          // The URL visited
    
    // Data Changes (JSON format)
    public string? OldValues { get; set; }     // State before change
    public string? NewValues { get; set; }     // State after change
}