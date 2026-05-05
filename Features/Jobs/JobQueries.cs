using Paramore.Darker;
using Microsoft.EntityFrameworkCore;
using Career635.Infrastructure.Persistence;
using Career635.Features.Jobs.Models;

namespace Career635.Features.Jobs;

public class GetHomeJobsQuery : IQuery<HomeViewModel> { }

public class GetHomeJobsHandler(AppDbContext context) : QueryHandlerAsync<GetHomeJobsQuery, HomeViewModel>
{
    public override async Task<HomeViewModel> ExecuteAsync(GetHomeJobsQuery query, CancellationToken ct = default)
    {
        // Use local time as requested for expiration logic
        var now = DateTime.UtcNow; 
        var weekAgo = now.AddDays(-7);
        var expiringThreshold = now.AddHours(48);

        // 1. Fetch Recent Jobs 
        // Filter: Must be Published AND PostedAt has passed AND hasn't expired yet
        var recent = await context.JobOpenings
            .AsNoTracking()
            .Where(j => j.Status == "Published" 
                     && j.PostedAt <= now 
                     && j.ExpiresAt >= now)
            .OrderByDescending(j => j.PostedAt)
            .Take(6)
            .Select(j => new HomeJobViewModel(
                j.Id, j.Title, j.WorkLocation, j.LocationType, 
                j.MinEducationLevel, j.RequiredExperienceYears, j.PostedAt, j.ExpiresAt))
            .ToListAsync(ct);

        // 2. Fetch Expiring Jobs (Urgent)
        // Filter: Published AND PostedAt passed AND expires within next 48 hours
        var expiring = await context.JobOpenings
            .AsNoTracking()
            .Where(j => j.Status == "Published" 
                     && j.PostedAt <= now
                     && j.ExpiresAt <= expiringThreshold 
                     && j.ExpiresAt > now).Take(6)
            .OrderBy(j => j.ExpiresAt)
            .Select(j => new HomeJobViewModel(
                j.Id, j.Title, j.WorkLocation, j.LocationType, 
                j.MinEducationLevel, j.RequiredExperienceYears, j.PostedAt, j.ExpiresAt))
            .ToListAsync(ct);

        // 3. Dynamic Categories from currently active jobs only
        var categories = await context.JobOpenings
            .Where(j => j.Status == "Published" && j.PostedAt <= now && j.ExpiresAt >= now)
            .Select(j => j.MinEducationLevel)
            .Distinct()
            .ToListAsync(ct);

        // 4. Global Stats (Active vs New)
        var totalActive = await context.JobOpenings.CountAsync(j => j.Status == "Published" && j.PostedAt <= now && j.ExpiresAt >= now, ct);
        var newThisWeek = await context.JobOpenings.CountAsync(j => j.Status == "Published" && j.PostedAt <= now && j.PostedAt >= weekAgo && j.ExpiresAt > now, ct);

        return new HomeViewModel(recent, expiring, categories, newThisWeek, totalActive);
    }
}

// --- SEARCH QUERY ---
public class GetJobSearchQuery : IQuery<SearchPageViewModel> {
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 9; // Grid of 3x3
}

public class GetJobSearchHandler(AppDbContext context,IConfiguration configuration) : QueryHandlerAsync<GetJobSearchQuery, SearchPageViewModel>
{
    public override async Task<SearchPageViewModel> ExecuteAsync(GetJobSearchQuery query, CancellationToken ct = default)
    {
         var now = DateTime.UtcNow;
        
        // Base query for counting (works globally regardless of SQL version)
        var baseQuery = context.JobOpenings.AsNoTracking()
            .Where(j => j.Status == "Published" && j.PostedAt <= now && j.ExpiresAt > now);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm)) 
        {
            var term = query.SearchTerm.ToLower();
            baseQuery = baseQuery.Where(j => j.Title.ToLower().Contains(term) || j.Description.ToLower().Contains(term));
        }

        List<JobSearchResultViewModel> results;

        var totalCount = await baseQuery.CountAsync(ct);
                var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
        bool useLegacyPagination = configuration.GetValue<bool>("DatabaseSettings:LegacyPagination", false);

        // var results = await dbQuery.OrderByDescending(j => j.PostedAt)
        //     .Skip((query.PageNumber - 1) * query.PageSize)
        //     .Take(query.PageSize)
        //     .Select(j => new JobSearchResultViewModel(
        //         j.Id, j.Title, j.JobCategory, j.WorkLocation, j.LocationType, 
        //         j.MinEducationLevel, j.RequiredExperienceYears, j.PostedAt, j.ExpiresAt))
        //     .ToListAsync(ct);
 if (useLegacyPagination)
        {
            // --- MSSQL 2008 COMPATIBLE PAGINATION (Using ROW_NUMBER) ---
            int skip = (query.PageNumber - 1) * query.PageSize;
            int takeBound = skip + query.PageSize;
            string? searchTermParam = string.IsNullOrWhiteSpace(query.SearchTerm) ? null : query.SearchTerm;

            // EF Core safely parametrizes {0}, {1}, etc., protecting against SQL Injection.
            // Extra columns like 'RowNum' are safely ignored by EF Core when mapping back to the Entity.
            var rawSql = @"
                SELECT * 
                FROM (
                    SELECT *, ROW_NUMBER() OVER (ORDER BY PostedAt DESC) AS RowNum
                    FROM JobOpenings
                    WHERE Status = 'Published' 
                      AND PostedAt <= {0} 
                      AND ExpiresAt > {0}
                      AND ({1} IS NULL OR LOWER(Title) LIKE '%' + LOWER({1}) + '%' OR LOWER(Description) LIKE '%' + LOWER({1}) + '%')
                ) AS PagedResults
                WHERE RowNum > {2} AND RowNum <= {3}";

            var legacyJobs = await context.JobOpenings
                .FromSqlRaw(rawSql, now, searchTermParam, skip, takeBound)
                .AsNoTracking()
                .ToListAsync(ct);

            results = legacyJobs.Select(j => new JobSearchResultViewModel(
                j.Id, j.Title, j.JobCategory, j.WorkLocation, j.LocationType, 
                j.MinEducationLevel, j.RequiredExperienceYears, j.PostedAt, j.ExpiresAt))
                .ToList();
        }
        else
        {
            // --- MODERN PAGINATION (MSSQL 2012+ / PostgreSQL) ---
            results = await baseQuery.OrderByDescending(j => j.PostedAt)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(j => new JobSearchResultViewModel(
                    j.Id, j.Title, j.JobCategory, j.WorkLocation, j.LocationType, 
                    j.MinEducationLevel, j.RequiredExperienceYears, j.PostedAt, j.ExpiresAt))
                .ToListAsync(ct);
        }

        return new SearchPageViewModel(results, query.SearchTerm, totalCount, query.PageNumber, totalPages);
    }
}
// --- DETAIL QUERY ---
public class GetJobDetailQuery : IQuery<JobDetailViewModel?> {
    public Guid Id { get; set; }
}

public class GetJobDetailHandler : QueryHandlerAsync<GetJobDetailQuery, JobDetailViewModel?>
{
    private readonly AppDbContext _context;
    public GetJobDetailHandler(AppDbContext context) => _context = context;

public override async Task<JobDetailViewModel?> ExecuteAsync(GetJobDetailQuery query, CancellationToken ct = default)
{
    var j = await _context.JobOpenings
        .AsNoTracking()
        .Include(x => x.RequiredSkills) // Ensure skills are loaded
        .FirstOrDefaultAsync(x => x.Id == query.Id, ct);

    if (j == null) return null;

    return new JobDetailViewModel(
        j.Id, j.Title,j.JobCategory,j.EmploymentType,j.TotalPositions, j.Description, j.Requirements,j.Benefits ,j.WorkLocation, j.LocationType, 
        j.MinAge, j.MaxAge,j.SalaryGrade, j.MinEducationLevel, j.RequiredMajorField, j.IsPecRequired, 
        j.RequiredExperienceYears, j.PostedAt, j.ExpiresAt, 
        j.ExpiresAt < DateTime.UtcNow,
        j.RequiredSkills.Select(s => s.SkillName).ToList()
    );
}
}


public class GetTrackStatusQuery : IQuery<TrackResultViewModel?> 
{
    public string TrackingCode { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5; // Detail-heavy rows, keep count low per page
}


public class GetTrackStatusHandler(AppDbContext _context) : QueryHandlerAsync<GetTrackStatusQuery, TrackResultViewModel?>
{
    public override async Task<TrackResultViewModel?> ExecuteAsync(GetTrackStatusQuery query, CancellationToken ct = default)
    {
        var input = query.TrackingCode?.Trim() ?? "";
        if (string.IsNullOrEmpty(input)) return null;

        // 1. Build Base Query - Search BOTH CNIC and TrackingCode columns
        // This ensures that if it matches either, we get the results.
        var baseQuery = _context.JobApplications.AsNoTracking()
            .Where(x => x.Applicant.CNICNumber == input || x.Applicant.TrackingCode == input);

        // 2. Get Total Count for Pagination
        var totalCount = await baseQuery.CountAsync(ct);
        if (totalCount == 0) return null;

        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        // 3. Fetch Paginated Results with ALL Relationships
        var data = await baseQuery
            .Include(ja => ja.JobOpening)
            .Include(ja => ja.Applicant).ThenInclude(a => a.PersonalInfo)
            .Include(ja => ja.Applicant).ThenInclude(a => a.FamilySummary)
            .Include(ja => ja.Applicant).ThenInclude(a => a.FinancialDetail)
            .Include(ja => ja.Applicant).ThenInclude(a => a.MilitaryDetail)
            .Include(ja => ja.Applicant).ThenInclude(a => a.Educations)
            .Include(ja => ja.Applicant).ThenInclude(a => a.Experiences)
            .Include(ja => ja.Applicant).ThenInclude(a => a.Siblings)
            .Include(ja => ja.Applicant).ThenInclude(a => a.InternalRelatives)
            .Include(ja => ja.Applicant).ThenInclude(a => a.Certifications)
            .Include(ja => ja.Applicant).ThenInclude(a => a.Skills)
            .Include(ja => ja.Applicant).ThenInclude(a => a.Achievements)
            .OrderByDescending(x => x.AppliedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        // 4. Map to ViewModels (Ensure logic handles 0 or nulls)
        var apps = data.Select(ja => new ApplicationStatusViewModel(
            ja.Id, 
            ja.JobOpening.Title, 
            ja.AppliedAt, 
            ja.Status, 
            ja.RecruiterRemarks, 
            ja.Applicant.TrackingCode,
            DateTime.UtcNow>ja.JobOpening.ExpiresAt,
            new ApplicantDossierViewModel(
                ja.Applicant.FullName, 
                ja.Applicant.CNICNumber, 
                ja.Applicant.PassportImageUrl,
                new PersonalInfoVM(ja.Applicant.PersonalInfo.FatherName, ja.Applicant.PersonalInfo.FatherCNIC, ja.Applicant.PersonalInfo.DateOfBirth, ja.Applicant.PersonalInfo.Gender, ja.Applicant.PersonalInfo.MaritalStatus, ja.Applicant.PersonalInfo.Religion, ja.Applicant.PersonalInfo.Caste, ja.Applicant.PersonalInfo.Sect, ja.Applicant.PersonalInfo.ContactNo, ja.Applicant.PersonalInfo.Email, ja.Applicant.PersonalInfo.PECNumber, ja.Applicant.PersonalInfo.PresentAddress, ja.Applicant.PersonalInfo.PermanentAddress, ja.Applicant.PersonalInfo.Accommodation),
                new FamilyVM(ja.Applicant.FamilySummary.BrothersTotal, ja.Applicant.FamilySummary.BrothersMarried, ja.Applicant.FamilySummary.BrothersUnmarried, ja.Applicant.FamilySummary.SistersTotal, ja.Applicant.FamilySummary.SistersMarried, ja.Applicant.FamilySummary.SistersUnmarried, ja.Applicant.FamilySummary.ChildrenTotal, ja.Applicant.FamilySummary.ChildrenMarried, ja.Applicant.FamilySummary.ChildrenUnmarried),
                new FinancialVM(ja.Applicant.FinancialDetail.CurrentSalary, ja.Applicant.FinancialDetail.ExpectedSalary, ja.Applicant.FinancialDetail.OtherBenefits, ja.Applicant.FinancialDetail.FamilyIncomeDetail, ja.Applicant.FinancialDetail.OtherFacilities),
                new MilitaryVM(ja.Applicant.MilitaryDetail.ArmyNumber, ja.Applicant.MilitaryDetail.ArmyUnit, ja.Applicant.MilitaryDetail.ArmyCharacter, ja.Applicant.MilitaryDetail.ArmyPayScale),
                ja.Applicant.Educations.Select(e => new EducationVM(e.Qualification, e.MajorField, e.BoardUniversity, e.CgpaPercentage, e.FromDate, e.ToDate)).ToList(),
                ja.Applicant.Experiences.Select(e => new ExperienceVM(e.OrganizationName, e.Designation, e.KeyResponsibilities, e.FromDate, e.ToDate)).ToList(),
                ja.Applicant.Siblings.Select(s => new SiblingVM(s.Name, s.CNIC, s.Gender, s.Occupation, s.MaritalStatus)).ToList(),
                ja.Applicant.InternalRelatives.Select(r => new RelativeVM(r.RelativeName, r.Designation, r.Department, r.PayScale)).ToList(),
                ja.Applicant.Certifications.Select(c => new CertVM(c.CertificateName, c.IssuingBody)).ToList(),
                ja.Applicant.Skills.Select(s => new SkillVM(s.SkillName, s.Proficiency)).ToList(),
                ja.Applicant.Achievements.Select(a => new AchVM(a.Title, a.Description, a.DateReceived)).ToList()
            )
        )).ToList();

        // Check if the input used was a CNIC to set the flag correctly (for UI messages)
        bool isCnicSearch = input.Contains("-") && !input.StartsWith("C635");

        return new TrackResultViewModel(apps, isCnicSearch, query.PageNumber, totalPages, totalCount);
    }
}