// Areas/Admin/Controllers/SupportController.cs
using Career635.Areas.Admin.Models;
using Career635.Domain.Entities.Auth;
using Career635.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Career635.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")]
[Authorize]
public class SupportController : Controller
{
    private readonly AppDbContext _context;

    public SupportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("ticket/{id:guid}")]
    public async Task<IActionResult> TicketDetail(Guid id)
    {
        var ticket = await _context.SupportTickets
            .FirstOrDefaultAsync(t => t.Id == id);
        
        if (ticket == null) return NotFound();

        var viewModel = new SupportTicketViewModel
        {
            Id = ticket.Id,
            Email = ticket.Email,
            Message = ticket.Message,
            Status = ticket.Status,
            IPAddress = ticket.IPAddress,
            UserAgent = ticket.UserAgent,
    
            CreatedAt = ticket.CreatedAt.UtcDateTime
        };

        // Auto-mark notification as read when viewing ticket
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var notification = await _context.UserNotifications
            .FirstOrDefaultAsync(n => n.ActionUrl == $"/admin/support/ticket/{id}" && n.UserId == userId && !n.IsRead);
        
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }

        return View(viewModel);
    }
// Areas/Admin/Controllers/SupportController.cs (Updated List Action)

[HttpGet("list")]
public async Task<IActionResult> List(string? status = null, string? search = null, int page = 1, int pageSize = 20)
{
    var query = _context.SupportTickets.AsQueryable();
    
    // Apply status filter
    if (!string.IsNullOrEmpty(status) && status != "All")
    {
        query = query.Where(t => t.Status == status);
    }
    
    // Apply search filter
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(t => t.Email.Contains(search) || t.Message.Contains(search));
    }
    
    // Get counts for status badges
    ViewBag.OpenCount = await _context.SupportTickets.CountAsync(t => t.Status == "Open");
    ViewBag.InProgressCount = await _context.SupportTickets.CountAsync(t => t.Status == "InProgress");
    ViewBag.ResolvedCount = await _context.SupportTickets.CountAsync(t => t.Status == "Resolved");
    ViewBag.ClosedCount = await _context.SupportTickets.CountAsync(t => t.Status == "Closed");
    
    var totalCount = await query.CountAsync();
    var tickets = await query
        .OrderByDescending(t => t.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(t => new SupportTicketViewModel
        {
            Id = t.Id,
            Email = t.Email,
            Message = t.Message.Length > 100 ? t.Message.Substring(0, 100) + "..." : t.Message,
            Status = t.Status,
            CreatedAt = t.CreatedAt.DateTime,
            IPAddress = t.IPAddress,
            UserAgent = t.UserAgent
        })
        .ToListAsync();
    
    ViewBag.CurrentStatus = status;
    ViewBag.SearchTerm = search;
    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    ViewBag.TotalCount = totalCount;
    ViewBag.StatusOptions = new[] { "All", "Open", "InProgress", "Resolved", "Closed" };
    
    return View(tickets);
}
    [HttpPost("ticket/{id:guid}/update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateTicket(Guid id, string status)
    {
        var ticket = await _context.SupportTickets.FindAsync(id);
        if (ticket == null) return NotFound();

        // Update status
        ticket.Status = status;
     
        await _context.SaveChangesAsync();
        
        TempData["Success"] = $"Ticket #{ticket.Id.ToString().Substring(0, 8)} updated successfully";
        return RedirectToAction(nameof(TicketDetail), new { id });
    }

 
    
    [HttpPost("ticket/{id:guid}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        var ticket = await _context.SupportTickets.FindAsync(id);
        if (ticket == null) return NotFound();
        
        _context.SupportTickets.Remove(ticket);
        await _context.SaveChangesAsync();
        
        TempData["Success"] = "Ticket deleted successfully";
        return RedirectToAction(nameof(List));
    }
}