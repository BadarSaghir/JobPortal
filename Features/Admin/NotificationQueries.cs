using Paramore.Darker;
using Microsoft.EntityFrameworkCore;
using Career635.Infrastructure.Persistence;
using Career635.Areas.Admin.Models;
using Career635.Domain.Entities.Auth;

namespace Career635.Features.Admin;

// 1. THE QUERY DEFINITION
// Returns the data needed for the header bell and dropdown
public class GetNotificationsQuery : IQuery<NotificationHeaderViewModel>
{
    public Guid UserId { get; set; }
}

// 2. THE HANDLER (THE LOGIC)
public class GetNotificationsHandler(AppDbContext context) 
    : QueryHandlerAsync<GetNotificationsQuery, NotificationHeaderViewModel>
{
    public override async Task<NotificationHeaderViewModel> ExecuteAsync(
        GetNotificationsQuery query, 
        CancellationToken ct = default)
    {
        // A. Fetch the 5 most recent non-deleted notifications
        var notifications = await context.Set<UserNotification>()
            .AsNoTracking()
            .Where(n => n.UserId == query.UserId && !n.IsDeleted)
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .ToListAsync(ct);

        // B. Get total unread count for the red badge
        var unreadCount = await context.Set<UserNotification>()
            .CountAsync(n => n.UserId == query.UserId && !n.IsRead && !n.IsDeleted, ct);

        // C. Transform entities to DTOs with "Time Ago" logic
        var dtos = notifications.Select(n => new NotificationDto(
            n.Id,
            n.Title,
            n.Message,
            n.ActionUrl,
            n.Type,
            n.IsRead,
            GetTimeAgo(n.CreatedAt.DateTime) // Human-readable time
        ));

        return new NotificationHeaderViewModel(dtos, unreadCount);
    }

    /// <summary>
    /// Professional utility to convert DateTime to human-readable format
    /// Matches the "Matte Emerald" premium feel.
    /// </summary>
    private static string GetTimeAgo(DateTime dateTime)
    {
        var span = DateTime.Now - dateTime;
        if (span.TotalMinutes < 1) return "Just now";
        if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
        if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
        if (span.TotalDays < 7) return $"{(int)span.TotalDays}d ago";
        return dateTime.ToString("MMM dd");
    }
}



public class GetPagedNotificationsQuery : IQuery<NotificationIndexViewModel>
{
    public Guid UserId { get; set; }
    public int Page { get; set; } = 1;
    public string? TypeFilter { get; set; }
    public bool? ReadFilter { get; set; }
}

public class GetPagedNotificationsHandler(AppDbContext context) 
    : QueryHandlerAsync<GetPagedNotificationsQuery, NotificationIndexViewModel>
{
    private const int PageSize = 15;

    public override async Task<NotificationIndexViewModel> ExecuteAsync(GetPagedNotificationsQuery query, CancellationToken ct)
    {
        var dbQuery = context.Set<UserNotification>()
            .AsNoTracking()
            .Where(n => n.UserId == query.UserId && !n.IsDeleted);

        // Filter by Type (Success, Warning, etc.)
        if (!string.IsNullOrEmpty(query.TypeFilter))
            dbQuery = dbQuery.Where(n => n.Type == query.TypeFilter);

        // Filter by Read Status
        if (query.ReadFilter.HasValue)
            dbQuery = dbQuery.Where(n => n.IsRead == query.ReadFilter.Value);

        var totalCount = await dbQuery.CountAsync(ct);
        var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

        var items = await dbQuery
            .OrderByDescending(n => n.CreatedAt)
            .Skip((query.Page - 1) * PageSize)
            .Take(PageSize)
            .Select(n => new NotificationDto(
                n.Id, n.Title, n.Message, n.ActionUrl, n.Type, n.IsRead, n.CreatedAt.ToString("MMM dd, yyyy HH:mm")
            ))
            .ToListAsync(ct);

        return new NotificationIndexViewModel(items, query.Page, totalPages, query.TypeFilter, query.ReadFilter, totalCount);
    }
}