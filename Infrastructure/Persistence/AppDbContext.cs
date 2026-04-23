using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Career635.Domain.Common;
using Career635.Domain.Entities.Auth;
using Career635.Domain.Entities.Applicants;
using Career635.Domain.Entities.Jobs;
using Career635.Domain.Entities.Locations;

namespace Career635.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<
    ApplicationUser, 
    ApplicationRole, 
    Guid, 
    IdentityUserClaim<Guid>, 
    IdentityUserRole<Guid>, 
    IdentityUserLogin<Guid>, 
    IdentityRoleClaim<Guid>, 
    IdentityUserToken<Guid>>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // --- AUTH & MASTER DATA ---
    public DbSet<ApplicationPermission> Permissions => Set<ApplicationPermission>();
    public DbSet<ApplicationRolePermission> RolePermissions => Set<ApplicationRolePermission>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Designation> Designations => Set<Designation>();
    public DbSet<PayScale> PayScales => Set<PayScale>();
    public DbSet<DegreeLevel> DegreeLevels => Set<DegreeLevel>();
    // --- JOB MANAGEMENT ---
    public DbSet<RecruitmentCampaign> RecruitmentCampaigns => Set<RecruitmentCampaign>();
    public DbSet<JobOpening> JobOpenings => Set<JobOpening>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<JobSkillRequirement> JobSkillRequirements => Set<JobSkillRequirement>();
    public DbSet<ApplicationStatusHistory> ApplicationStatusHistories => Set<ApplicationStatusHistory>();
    public DbSet<CampaignExportTask> CampaignExportTasks => Set<CampaignExportTask>();

    // --- APPLICANT DATA (8-PAGE WIZARD) ---
    public DbSet<Applicant> Applicants => Set<Applicant>();
    
    public DbSet<ApplicantPersonalInfo> ApplicantPersonalInfos => Set<ApplicantPersonalInfo>();
    public DbSet<ApplicantFamilySummary> ApplicantFamilySummaries => Set<ApplicantFamilySummary>();
    public DbSet<ApplicantFinancialDetail> ApplicantFinancialDetails => Set<ApplicantFinancialDetail>();
    public DbSet<ApplicantMilitaryDetail> ApplicantMilitaryDetails => Set<ApplicantMilitaryDetail>();
    public DbSet<ApplicantEducation> ApplicantEducations => Set<ApplicantEducation>();
    public DbSet<ApplicantExperience> ApplicantExperiences => Set<ApplicantExperience>();
    public DbSet<ApplicantSibling> ApplicantSiblings => Set<ApplicantSibling>();
    public DbSet<ApplicantInternalRelative> ApplicantInternalRelatives => Set<ApplicantInternalRelative>();
    public DbSet<ApplicantCertification> ApplicantCertifications => Set<ApplicantCertification>();
    public DbSet<ApplicantSkill> ApplicantSkills => Set<ApplicantSkill>();
    public DbSet<ApplicantAchievement> ApplicantAchievements => Set<ApplicantAchievement>();
    public DbSet<ApplicantDocument> ApplicantDocuments => Set<ApplicantDocument>();

    // --- LOCATION SYSTEM ---
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Province> Provinces => Set<Province>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<City> Tehsils => Set<City>();
    public DbSet<Address> Addresses => Set<Address>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // 1. Mandatory Identity Base Call
        base.OnModelCreating(builder);

        // 2. AUTOMATIC CONFIGURATION LOADING
        // This scans your 'Configurations' folder for any class implementing IEntityTypeConfiguration
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // 3. GLOBAL SOFT DELETE QUERY FILTER
        // Automatically filters out records where IsDeleted == true in every query
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted)), 
                    Expression.Constant(false));
                builder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // 4. SOFT DELETE INTERCEPTION
        // Instead of hard-deleting, we update the IsDeleted flag and timestamp
        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.State = EntityState.Modified; // Intercept Delete
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTimeOffset.UtcNow;
                    break;
            }
        }
        return base.SaveChangesAsync(ct);
    }
}