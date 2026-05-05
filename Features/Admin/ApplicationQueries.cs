using Career635.Areas.Admin.Models;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Darker;

public class GetApplicationReviewQuery : IQuery<ApplicationReviewViewModel?> 
{ 
    public Guid Id { get; set; } 
}

public class GetApplicationReviewHandler(AppDbContext context) : QueryHandlerAsync<GetApplicationReviewQuery, ApplicationReviewViewModel?>
{
    public override async Task<ApplicationReviewViewModel?> ExecuteAsync(GetApplicationReviewQuery query, CancellationToken ct = default)
    {
        var app = await context.JobApplications
            .AsNoTracking()
            .Include(a => a.JobOpening)
            .Include(a => a.Applicant).ThenInclude(x => x.PersonalInfo)
            .Include(a => a.Applicant).ThenInclude(x => x.FamilySummary)
            .Include(a => a.Applicant).ThenInclude(x => x.FinancialDetail)
            .Include(a => a.Applicant).ThenInclude(x => x.MilitaryDetail)
            .Include(a => a.Applicant).ThenInclude(x => x.Educations)
            .Include(a => a.Applicant).ThenInclude(x => x.Experiences)
            .Include(a => a.Applicant).ThenInclude(x => x.Siblings)
            .Include(a => a.Applicant).ThenInclude(x => x.InternalRelatives)
            .Include(a => a.Applicant).ThenInclude(x => x.Certifications)
            .FirstOrDefaultAsync(a => a.Id == query.Id, ct);

        if (app == null) return null;

        return new ApplicationReviewViewModel(
            app.Id, app.Status, app.RecruiterRemarks, app.Applicant, 
            app.JobOpening.Title, app.AppliedAt);
    }
}