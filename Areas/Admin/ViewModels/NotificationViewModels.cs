namespace Career635.Areas.Admin.Models;

public record NotificationDto(
    Guid Id,
    string Title,
    string Message,
    string? ActionUrl,
    string Type,
    bool IsRead,
    string TimeAgo
);

public record NotificationHeaderViewModel(
    IEnumerable<NotificationDto> RecentNotifications,
    int UnreadCount
);



public record NotificationIndexViewModel(
    IEnumerable<NotificationDto> Items,
    int PageNumber,
    int TotalPages,
    string? TypeFilter,
    bool? ReadFilter,
    int TotalCount
) {
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}