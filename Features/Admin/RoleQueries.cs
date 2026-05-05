using Career635.Areas.Admin.Models;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Darker;

public class GetPagedRolesQuery : IQuery<RoleIndexViewModel> 
{ 
    public int Page { get; set; } = 1; 
}

public class GetPagedRolesHandler(AppDbContext context) : QueryHandlerAsync<GetPagedRolesQuery, RoleIndexViewModel>
{
    private const int PageSize = 10;

    public override async Task<RoleIndexViewModel> ExecuteAsync(GetPagedRolesQuery query, CancellationToken ct = default)
    {
        var dbQuery = context.Roles.AsNoTracking().Where(r => !r.IsDeleted);

        var totalCount = await dbQuery.CountAsync(ct);
        var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

        var items = await dbQuery
            .OrderBy(r => r.Name)
            .Skip((query.Page - 1) * PageSize)
            .Take(PageSize)
            .Select(r => new RoleSummaryViewModel(
                r.Id,
                r.Name!,
                r.Description,
                context.RolePermissions.Count(rp => rp.RoleId == r.Id),
                context.UserRoles.Count(ur => ur.RoleId == r.Id)
            ))
            .ToListAsync(ct);

        return new RoleIndexViewModel(items, query.Page, totalPages, totalCount);
    }
}