using Career635.Areas.Admin.Models;
using Career635.Domain.Entities.Auth;
using Career635.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Paramore.Darker;

public class GetPagedStaffQuery : IQuery<StaffIndexViewModel>
{
    public string? SearchTerm { get; set; }
    public Guid? DepartmentId { get; set; }
    public int Page { get; set; } = 1;
}

public class GetPagedStaffHandler(AppDbContext context, UserManager<ApplicationUser> userManager) 
    : QueryHandlerAsync<GetPagedStaffQuery, StaffIndexViewModel>
{
    public override async Task<StaffIndexViewModel> ExecuteAsync(GetPagedStaffQuery query, CancellationToken ct = default)
    {
        var dbQuery = context.Users
            .AsNoTracking()
            .Include(u => u.Department)
            .Include(u => u.Designation)
            .Where(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            dbQuery = dbQuery.Where(u => u.FullName.Contains(query.SearchTerm) || u.Email!.Contains(query.SearchTerm));

        if (query.DepartmentId.HasValue)
            dbQuery = dbQuery.Where(u => u.DepartmentId == query.DepartmentId);

        var totalCount = await dbQuery.CountAsync(ct);
        var users = await dbQuery
            .OrderByDescending(u => u.CreatedAt)
            .Skip((query.Page - 1) * 10).Take(10)
            .ToListAsync(ct);

        var items = new List<StaffSummaryViewModel>();
        foreach (var u in users)
        {
            var roles = await userManager.GetRolesAsync(u);
            items.Add(new StaffSummaryViewModel(u.Id, u.FullName, u.Email!, u.Department?.Name, u.Designation?.Title, roles, u.CreatedAt.DateTime));
        }

        return new StaffIndexViewModel(items, query.Page, (int)Math.Ceiling(totalCount / 10.0), query.SearchTerm, query.DepartmentId);
    }
}