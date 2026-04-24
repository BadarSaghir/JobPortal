using Microsoft.EntityFrameworkCore.ChangeTracking;
using Career635.Domain.Entities.Auth;
using System.Text.Json;

namespace Career635.Infrastructure.Persistence;

public class AuditEntry(EntityEntry entry)
{
    public EntityEntry Entry { get; } = entry;
    public string TableName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? IPAddress { get; set; }
    public Dictionary<string, object?> KeyValues { get; } = new();
    public Dictionary<string, object?> OldValues { get; } = new();
    public Dictionary<string, object?> NewValues { get; } = new();
    public List<PropertyEntry> TemporaryProperties { get; } = new();

    public bool HasChanges => OldValues.Count > 0 || NewValues.Count > 0;

    public AuditLog ToAuditLog()
    {
        var audit = new AuditLog
        {
            UserId = UserId,
            UserName = UserName,
            Action = Action,
            EntityName = TableName,
            CreatedAt = DateTimeOffset.UtcNow,
            IPAddress = IPAddress,
            // Serialize dictionaries to JSON strings for the DB
            OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
            // Join keys for reference (e.g. "Id: 36f86...")
            EntityId = JsonSerializer.Serialize(KeyValues)
        };
        return audit;
    }
}