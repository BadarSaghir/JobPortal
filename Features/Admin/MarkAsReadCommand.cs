// Features/Admin/MarkAsReadCommand.cs
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Brighter;

namespace Career635.Features.Admin;

public class MarkAsReadCommand(Guid notificationId) :Command(new Id(Guid.NewGuid().ToString()))
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid NotificationId { get; set; } = notificationId;
}

public class MarkAsReadHandler(AppDbContext context) : RequestHandlerAsync<MarkAsReadCommand>
{
    public override async Task<MarkAsReadCommand> HandleAsync(MarkAsReadCommand command, CancellationToken ct = default)
    {
        var notification = await context.UserNotifications
            .FirstOrDefaultAsync(n => n.Id == command.NotificationId, ct);

        if (notification != null && !notification.IsRead)
        {
            notification.IsRead = true;
            await context.SaveChangesAsync(ct);
        }
        return await base.HandleAsync(command, ct);
    }
}