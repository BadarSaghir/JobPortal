using Paramore.Brighter;
using Career635.Infrastructure.Persistence;
using Career635.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace Career635.Features.Support;

public class SubmitSupportCommand(string email, string message, string ip, string agent) : Command(new Id(Guid.NewGuid().ToString()))
{
    public string Email { get; } = email;
    public string Message { get; } = message;
    public string IP { get; } = ip;
    public string Agent { get; } = agent;
}

public class SubmitSupportHandler(AppDbContext context) : RequestHandlerAsync<SubmitSupportCommand>
{
    public override async Task<SubmitSupportCommand> HandleAsync(SubmitSupportCommand command, CancellationToken ct = default)
    {
        // 1. Save Ticket
        var ticket = new SupportTicket {
            Email = command.Email,
            Message = command.Message,
            IPAddress = command.IP,
            UserAgent = command.Agent
        };
        context.Set<SupportTicket>().Add(ticket);

        // 2. Create System Notification for Admins
        var admins = await context.Users.ToListAsync(ct); // In real app, filter by SuperAdmin role
        foreach(var admin in admins) {
            context.Set<UserNotification>().Add(new UserNotification {
                UserId = admin.Id,
                Title = "New Technical Support Request",
                Message = $"Query from {command.Email}: {command.Message.Substring(0, Math.Min(command.Message.Length, 50))}...",
                Type = "Urgent"
            });
        }

        await context.SaveChangesAsync(ct);
        return await base.HandleAsync(command, ct);
    }
}