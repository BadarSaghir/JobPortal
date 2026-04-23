using Paramore.Brighter;
using Career635.Infrastructure.Persistence;
using Career635.Domain.Entities.Jobs;

namespace Career635.Features.Admin;

public class UpdateApplicationStatusHandler(AppDbContext context) : RequestHandlerAsync<UpdateApplicationStatusCommand>
{
    public override async Task<UpdateApplicationStatusCommand> HandleAsync(UpdateApplicationStatusCommand command, CancellationToken ct = default)
    {
        var application = await context.JobApplications.FindAsync(command.ApplicationId);
        if (application == null) throw new KeyNotFoundException("Application dossier not found.");

        // 1. Update the Main Status
        application.Status = command.NewStatus;
        application.RecruiterRemarks = command.Remarks;

        // 2. Add to History (Audit Trail)
        var history = new ApplicationStatusHistory
        {
            JobApplicationId = application.Id,
            Status = command.NewStatus,
            Remarks = command.Remarks,
            ChangedByUserId = command.AdminUserId
        };

        context.ApplicationStatusHistories.Add(history);
        await context.SaveChangesAsync(ct);

        return await base.HandleAsync(command, ct);
    }
}