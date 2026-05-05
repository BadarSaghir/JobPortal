// Areas/Admin/Models/SupportTicketViewModel.cs
namespace Career635.Areas.Admin.Models;

public class SupportTicketViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
  
    public DateTime CreatedAt { get; set; }
    public string TimeAgo => GetTimeAgo(CreatedAt);
    
    public string StatusBadgeClass => Status switch
    {
        "Open" => "bg-red-100 text-red-700 border-red-200",
        "InProgress" => "bg-yellow-100 text-yellow-700 border-yellow-200",
        "Resolved" => "bg-green-100 text-green-700 border-green-200",
        "Closed" => "bg-gray-100 text-gray-700 border-gray-200",
        _ => "bg-slate-100 text-slate-700 border-slate-200"
    };
    
    public string StatusButtonClass => Status switch
    {
        "Open" => "bg-red-600 hover:bg-red-700",
        "InProgress" => "bg-yellow-600 hover:bg-yellow-700",
        "Resolved" => "bg-green-600 hover:bg-green-700",
        "Closed" => "bg-gray-600 hover:bg-gray-700",
        _ => "bg-slate-600 hover:bg-slate-700"
    };
    
    private string GetTimeAgo(DateTime date)
    {
        var diff = DateTime.UtcNow - date;
        if (diff.Days > 0) return $"{diff.Days} day{(diff.Days > 1 ? "s" : "")} ago";
        if (diff.Hours > 0) return $"{diff.Hours} hour{(diff.Hours > 1 ? "s" : "")} ago";
        if (diff.Minutes > 0) return $"{diff.Minutes} minute{(diff.Minutes > 1 ? "s" : "")} ago";
        return "Just now";
    }
}