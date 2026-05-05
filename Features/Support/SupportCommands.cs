using Paramore.Brighter;
using Career635.Infrastructure.Persistence;
using Career635.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Career635.Domain.Constants;

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
            UserAgent = command.Agent,
            Status = "Open"
        };
        context.Set<SupportTicket>().Add(ticket);
        await context.SaveChangesAsync(ct); // Save to get ticket.Id

        // 2. Create System Notification for Admins
          // 2. Create System Notification ONLY for users with SystemAll permission
        // var admins = await context.RolePermissions.Include(p=>p.Permission)
        //     .Where(u => u.Permission.Name == AppPermissions.SystemAll)
        //     .ToListAsync(ct); // In real app, filter by SuperAdmin role
     var admins = from users in context.Users
             join userRoles in context.UserRoles on users.Id equals userRoles.UserId
             join rolePermissions in context.RolePermissions on userRoles.RoleId equals rolePermissions.RoleId
             join permissions in context.Permissions on rolePermissions.PermissionId equals permissions.Id
             where permissions.Name == AppPermissions.SystemAll  // WHERE clause for permission name
             select users;
             var adminUsers = await admins.Distinct().ToListAsync(ct);

        foreach(var admin in adminUsers) {
            context.Set<UserNotification>().Add(new UserNotification {
                UserId = admin.Id,
                Title = "New Technical Support Request",
                Message = $"Query from {command.Email}: {command.Message.Substring(0, Math.Min(command.Message.Length, 10))}...",
                Type = "Urgent",
                    ActionUrl = $"/admin/support/ticket/{ticket.Id}"  // Link to ticket detail, not notification

               
            });
        }

        await context.SaveChangesAsync(ct);
        return await base.HandleAsync(command, ct);
    }
}