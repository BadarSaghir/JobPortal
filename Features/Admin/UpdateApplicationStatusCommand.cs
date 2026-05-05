using Paramore.Brighter;

namespace Career635.Features.Admin;

public class UpdateApplicationStatusCommand : Command
{
    public Guid ApplicationId { get; }
    public string NewStatus { get; }
    public string? Remarks { get; }
    public Guid AdminUserId { get; }

    public UpdateApplicationStatusCommand(Guid applicationId, string newStatus, string? remarks, Guid adminUserId) 
        : base(new Id(Guid.NewGuid().ToString()))
    {
        ApplicationId = applicationId;
        NewStatus = newStatus;
        Remarks = remarks;
        AdminUserId = adminUserId;
    }
}