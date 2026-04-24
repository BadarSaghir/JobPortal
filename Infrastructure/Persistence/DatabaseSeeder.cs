using Career635.Domain.Constants;
using Career635.Domain.Entities.Applicants;
using Career635.Domain.Entities.Auth;
using Career635.Domain.Entities.Jobs;
using Career635.Domain.Entities.Locations;
using Career635.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class DatabaseSeeder{

private static async Task SeedMasterDataAsync(AppDbContext context)
{
    if (await context.Departments.AnyAsync()) return;

    var depts = new List<Department> {
        new() { Name = "Information Technology", Code = "IT" },
        new() { Name = "Human Resources", Code = "HR" },
        new() { Name = "Engineering & Production", Code = "ENG" }
    };
    await context.Departments.AddRangeAsync(depts);

    var desigs = new List<Designation> {
        new() { Title = "Senior Software Engineer" },
        new() { Title = "Project Manager" },
        new() { Title = "Assistant Director" }
    };
    await context.Designations.AddRangeAsync(desigs);

    var scales = new List<PayScale> {
        new() { Grade = "BPS-17", Description = "Officer Grade" },
        new() { Grade = "BPS-18", Description = "Senior Officer" }
    };
    await context.PayScales.AddRangeAsync(scales);
    await context.SaveChangesAsync();
}


private static async Task SeedLocationsAsync(AppDbContext context)
{
    if (await context.Countries.AnyAsync()) return;

    var country = new Country { Name = "Pakistan" };
    var province = new Province { Name = "Punjab", Country = country };
    var district = new District { Name = "Rawalpindi", Province = province };
    var tehsil = new City { Name = "Rawalpindi", District = district };

    await context.Tehsils.AddAsync(tehsil); // Cascades all parents

    // Seed a dummy address for the organization headquarters
    await context.Addresses.AddAsync(new Address {
        Country = country,
        Province = province,
        District = district,
        City = tehsil,
        StreetAddress = "NRTC HQ, Sector G-1, Industrial Area"
    });

    await context.SaveChangesAsync();
}


private static async Task SeedJobsAsync(AppDbContext context)
{
    if (await context.JobOpenings.AnyAsync()) return;

    var campaign = new RecruitmentCampaign { Name = "Annual Intake 2025", CampaignCode = "BATCH-25" };
    var dept = await context.Departments.FirstAsync();
    var desig = await context.Designations.FirstAsync();

    var job = new JobOpening {
        Campaign = campaign,

        Title = "Full Stack .NET Developer",
        Description = "Looking for experts in C# and Razor Views.",
        Requirements = "- 5 Years Experience\n- Masters Degree",
        MinAge = 24,
        MaxAge = 40,
        RequiredExperienceYears = 5,
        MinEducationLevel = "Masters",
        Status = "Published",
        PostedAt = DateTime.UtcNow.AddDays(-10),

        ExpiresAt = DateTime.UtcNow.AddDays(2) // CLOSING SOON
    };

    await context.JobOpenings.AddAsync(job);
    await context.SaveChangesAsync();
}
private static async Task SeedApplicantsAsync(AppDbContext context)
{
    if (await context.Applicants.AnyAsync()) return;

    var job = await context.JobOpenings.FirstAsync();
    var tehsil = await context.Tehsils.FirstAsync();
    var country = await context.Countries.FirstAsync();
    var province = await context.Provinces.FirstAsync();
    var district = await context.Districts.FirstAsync();

    var address = new Address { 
        CountryId = country.Id, ProvinceId = province.Id, 
        DistrictId = district.Id, TehsilId = tehsil.Id, 
        StreetAddress = "House 123, Street 4" 
    };

    var applicant = new Applicant {
        FullName = "Badar Developer",
        CNICNumber = "12345-6789012-3",
        TrackingCode = "C635-XYZ-2025",
        PersonalInfo = new ApplicantPersonalInfo {
            FatherName = "Senior Developer",
            DateOfBirth = new DateTime(1995, 5, 20),
            MaritalStatus = "Single",
            Religion = "Islam",
            ContactNo = "0300-1234567",
            CandidateType = "External",
            PresentAddress = "as",
            PermanentAddress = "sads"
        },
        FamilySummary = new ApplicantFamilySummary { BrothersTotal = 2, SistersTotal = 1, ChildrenTotal = 0 },
        FinancialDetail = new ApplicantFinancialDetail { CurrentSalary = 150000, ExpectedSalary = 200000 },
        MilitaryDetail = new ApplicantMilitaryDetail { ArmyNumber = "N/A" }
    };
    if (!await context.Set<DegreeLevel>().AnyAsync())
    {
        var levels = new List<DegreeLevel> {
            new() { Name = "Matric / O-Level", LevelOrder = 5 },
            new() { Name = "Intermediate / A-Level", LevelOrder = 4 },
            new() { Name = "Bachelors", LevelOrder = 3 },
            new() { Name = "Masters", LevelOrder = 2 },
            new() { Name = "PhD", LevelOrder = 1 }
        };
        await context.Set<DegreeLevel>().AddRangeAsync(levels);
    }
      var mastersLevel = await context.Set<DegreeLevel>().FirstAsync(x => x.Name == "Masters");
    await context.SaveChangesAsync();
   applicant.Educations.Add(new ApplicantEducation 
    { 
        DegreeLevelId = mastersLevel.Id, // Link to Master Data
        Qualification = "MS Computer Science", 
        MajorField = "Software Engineering", 
        BoardUniversity = "NUST", 
        CgpaPercentage = "3.8/4.0",
        FromDate = new DateTime(2016, 09, 01), // Start Date
        ToDate = new DateTime(2018, 06, 30)    // Completion Date
    });
    applicant.Experiences.Add(new ApplicantExperience { OrganizationName = "Tech Corp", Designation = "Mid Developer", FromDate = DateTime.UtcNow.AddYears(-5) });

    await context.Applicants.AddAsync(applicant);
    await context.SaveChangesAsync();

    // Create the Application Link
    await context.JobApplications.AddAsync(new JobApplication {
        JobOpeningId = job.Id,
        ApplicantId = applicant.Id,
        Status = "Pending",
        MatchScore = 95.50m
    });
    
    await context.SaveChangesAsync();
}
private static async Task SeedPermissionsAndSuperAdminAsync(
    AppDbContext context, 
    RoleManager<ApplicationRole> roleManager, 
    UserManager<ApplicationUser> userManager)
{
    // 1. Seed Granular Permissions
    var currentPermissions = await context.Permissions.ToListAsync();
    foreach (var p in AppPermissions.AllPermissions)
    {
        if (!currentPermissions.Any(x => x.Name == p.Name))
        {
            context.Permissions.Add(new ApplicationPermission 
            { 
                Name = p.Name, 
                Module = p.Module, 
                DisplayName = p.Display 
            });
        }
    }
    await context.SaveChangesAsync();

    // 2. Seed SuperAdmin Role
    var superAdminRoleName = "SuperAdmin";
    var superAdminRole = await roleManager.FindByNameAsync(superAdminRoleName);

    if (superAdminRole == null)
    {
        superAdminRole = new ApplicationRole(superAdminRoleName) 
        { 
            Description = "Root administrative group with total system control." 
        };
        await roleManager.CreateAsync(superAdminRole);
    }

    // 3. Link All Permissions to SuperAdmin Role
    var allPermissionIds = await context.Permissions.Select(p => p.Id).ToListAsync();
    var existingRolePerms = await context.RolePermissions
        .Where(rp => rp.RoleId == superAdminRole.Id)
        .Select(rp => rp.PermissionId)
        .ToListAsync();

    foreach (var pId in allPermissionIds)
    {
        if (!existingRolePerms.Contains(pId))
        {
            context.RolePermissions.Add(new ApplicationRolePermission
            {
                RoleId = superAdminRole.Id,
                PermissionId = pId
            });
        }
    }
    await context.SaveChangesAsync();

    // 4. Seed the Master SuperAdmin User
    var adminEmail = "admin@career635.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "System Architect",
            EmailConfirmed = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@635!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, superAdminRoleName);
        }
    }
}
public static async Task SeedAllAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

    // 0. Ensure Database is created
    await context.Database.EnsureCreatedAsync();

    // 1. Run Master Data & Locations
    await SeedMasterDataAsync(context);
    await SeedLocationsAsync(context);

    await SeedPermissionsAndSuperAdminAsync(context, roleManager, userManager);

    // 2. Seed Admin Identity



   if (await context.Set<DegreeLevel>().AnyAsync()) return;

    var levels = new List<DegreeLevel>
    {
        new() { Name = "PhD (Doctorate)", LevelOrder = 1 },
        new() { Name = "MPhil / MS (18 Years)", LevelOrder = 2 },
        new() { Name = "Masters / Bachelors (16 Years)", LevelOrder = 3 },
        new() { Name = "Intermediate / A-Level", LevelOrder = 4 },
        new() { Name = "Matric / O-Level", LevelOrder = 5 },
        new() { Name = "Middle", LevelOrder = 6 },
        new() { Name = "Primary", LevelOrder = 7 },
        new() { Name = "Other / Vocational", LevelOrder = 8 }
    };

    await context.Set<DegreeLevel>().AddRangeAsync(levels);
    await context.SaveChangesAsync();
    // 3. Run Business Data
    await SeedJobsAsync(context);
    await SeedApplicantsAsync(context);
}

}