// Features/Admin/MarkAllAsReadCommand.cs
using Career635.Infrastructure.Persistence;
using Career635.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Paramore.Brighter;

namespace Career635.Features.Admin;

public class MarkAllAsReadCommand(Guid userId) : Command(new Id(Guid.NewGuid().ToString()))

{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = userId;
}

public class MarkAllAsReadHandler(AppDbContext context) :  RequestHandlerAsync<MarkAllAsReadCommand>
{
    public override async Task<MarkAllAsReadCommand> HandleAsync(MarkAllAsReadCommand command, CancellationToken ct = default)
    {
        // High-performance bulk update
        await context.Set<UserNotification>()
            .Where(n => n.UserId == command.UserId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), ct);

        return await base.HandleAsync(command, ct);
    }
}