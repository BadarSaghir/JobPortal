// Features/Admin/GetNotificationDetailQuery.cs
using Career635.Areas.Admin.Models;
using Career635.Domain.Entities.Auth;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Darker;

namespace Career635.Features.Admin;

public class GetNotificationDetailQuery : IQuery<NotificationDto?>
{
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
}

public class GetNotificationDetailHandler(AppDbContext context) 
    : QueryHandlerAsync<GetNotificationDetailQuery, NotificationDto?>
{
    public override async Task<NotificationDto?> ExecuteAsync(GetNotificationDetailQuery query, CancellationToken ct = default)
    {
        var n = await context.UserNotifications
            .FirstOrDefaultAsync(x => x.Id == query.NotificationId && x.UserId == query.UserId, ct);

        if (n == null) return null;

        return new NotificationDto(
            n.Id, n.Title, n.Message, n.ActionUrl, n.Type, n.IsRead, n.CreatedAt.ToString("MMMM dd, yyyy HH:mm")
        );
    }
}