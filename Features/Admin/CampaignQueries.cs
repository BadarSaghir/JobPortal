using Paramore.Darker;
using Microsoft.EntityFrameworkCore;
using Career635.Infrastructure.Persistence;
using Career635.Areas.Admin.Models;
using Career635.Domain.Entities.Jobs;

namespace Career635.Features.Admin;

public class GetPagedCampaignsQuery : IQuery<CampaignIndexViewModel>
{
    public string? SearchTerm { get; set; }
    public bool? StatusFilter { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetPagedCampaignsHandler(AppDbContext context) 
    : QueryHandlerAsync<GetPagedCampaignsQuery, CampaignIndexViewModel>
{
    public override async Task<CampaignIndexViewModel> ExecuteAsync(GetPagedCampaignsQuery query, CancellationToken ct = default)
    {
        var dbQuery = context.RecruitmentCampaigns.AsNoTracking().Where(c => !c.IsDeleted);

        // 1. Apply Search (Name or Code)
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.ToLower();
            dbQuery = dbQuery.Where(c => c.Name.ToLower().Contains(term) || c.CampaignCode.ToLower().Contains(term));
        }

        // 2. Apply Status Filter
        if (query.StatusFilter.HasValue)
        {
            dbQuery = dbQuery.Where(c => c.IsActive == query.StatusFilter.Value);
        }

        var totalCount = await dbQuery.CountAsync(ct);

        // 3. Project and Paginate
        var items = await dbQuery
            .OrderByDescending(c => c.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(c => new CampaignSummaryViewModel(
                c.Id,
                c.Name,
                c.CampaignCode,
                c.IsActive,
                context.JobOpenings.Count(j => j.CampaignId == c.Id && !j.IsDeleted),
                c.CreatedAt.DateTime,
                context.Set<CampaignExportTask>()
                    .Where(t => t.CampaignId == c.Id)
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(t => t.Status).FirstOrDefault() ?? "None",
                context.Set<CampaignExportTask>()
                    .Where(t => t.CampaignId == c.Id && t.Status == "Completed")
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(t => t.DownloadUrl).FirstOrDefault()
            ))
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        return new CampaignIndexViewModel(items, query.Page, totalPages, query.SearchTerm, query.StatusFilter, totalCount);
    }
}