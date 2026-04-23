using Microsoft.AspNetCore.Mvc;
using Career635.Areas.Candidate.Models;
using Career635.Domain.Entities.Applicants;
using Mapster;
using Paramore.Brighter;
using Career635.Features.Applicants;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Career635.Areas.Candidate.Controllers;

[Area("Candidate")]
public class WizardController(
    IAmACommandProcessor _commandProcessor, 
    IFileStorageService _fileService,
    AppDbContext _context
    ) : Controller
{


    [HttpGet]
    public async Task<IActionResult> ApplyAsync(Guid jobId)
    {
           if (jobId == Guid.Empty) return RedirectToAction("Index", "Home", new { area = "" });

        // --- CHECK IF JOB IS OPEN ---
        var job = await _context.JobOpenings.FindAsync(jobId);
        
        if (job == null) return NotFound();

        // Logic: Must be "Published" AND not expired
        if (job.Status != "Published" || job.ExpiresAt < DateTime.UtcNow)
        {
            TempData["ErrorMessage"] = "We are sorry, but this job position is no longer accepting applications.";
            return RedirectToAction("Details", "Job", new { area = "", id = jobId });
        }


        // Initialize ViewModel with 5 empty slots for all collections
        var model = new ApplicationSubmitViewModel { JobId = jobId };
     

        return View(model);
    }

[HttpPost]
[ValidateAntiForgeryToken]
[RequestSizeLimit(5242880)] //

public async Task<IActionResult> Submit(ApplicationSubmitViewModel model)
{
    // 1. DATA CLEANING: Remove empty rows from dynamic lists
    // model.Educations.RemoveAll(x => string.IsNullOrWhiteSpace(x.Qualification));
    // model.Experiences.RemoveAll(x => string.IsNullOrWhiteSpace(x.OrganizationName));
    // model.Siblings.RemoveAll(x => string.IsNullOrWhiteSpace(x.Name));
    // model.InternalRelatives.RemoveAll(x => string.IsNullOrWhiteSpace(x.RelativeName));
    // model.Skills.RemoveAll(x => string.IsNullOrWhiteSpace(x.SkillName));
    // model.Certifications.RemoveAll(x => string.IsNullOrWhiteSpace(x.CertificateName));
    // model.Achievements.RemoveAll(x => string.IsNullOrWhiteSpace(x.Title));

    if (!ModelState.IsValid) return View("Apply", model);

    try
    {
                    // --- RE-VERIFY JOB STATUS BEFORE SAVING (Security Check) ---
            var job = await _context.JobOpenings
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == model.JobId);

            if (job == null || job.Status != "Published" || job.ExpiresAt < DateTime.UtcNow)
            {
                ModelState.AddModelError("", "This job opening has just closed or is no longer available.");
                return View("Apply", model);
            }
        // 2. Handle File Uploads
  var applicantId = Guid.NewGuid(); // Pre-generate ID for folder structure
var campid=job.CampaignId.ToString();
// 1. Process Core Files (CV/Photo)
var campdefaultId=!string.IsNullOrEmpty(campid)?campid:"Default";
var photoPath = await _fileService.SaveApplicantFileAsync(model.PassportPhoto, 
    model.JobId.ToString(), model.CNICNumber, applicantId.ToString(), "Profile",campdefaultId);

var cvPath = await _fileService.SaveApplicantFileAsync(model.CvFile, 
    model.JobId.ToString(), model.CNICNumber, applicantId.ToString(), "CV",campdefaultId);

// 2. Process Dynamic Documents
var documentEntities = new List<ApplicantDocument>();
foreach (var entry in model.DocumentAttachments)
{
    foreach (var file in entry.Files)
    {
        var filePath = await _fileService.SaveApplicantFileAsync(file, 
            model.JobId.ToString(), model.CNICNumber, applicantId.ToString(), entry.DocumentType,campdefaultId);

        documentEntities.Add(new ApplicantDocument {
            DocumentType = entry.DocumentType,
            FileUrl = filePath,
            UploadedAt = DateTimeOffset.UtcNow
        });
    }
}
// re-generate ID for folder structure

        // 3. MANUAL MAPPING (Fixes the "missing data" issue)
        var applicant = new Applicant
        {
            FullName = model.FullName,
            CNICNumber = model.CNICNumber,
            PassportImageUrl = photoPath,
            CvUrl = cvPath,

            // Map 1:1 Personal Info
            PersonalInfo = new ApplicantPersonalInfo
            {
                FatherName = model.FatherName,
                FatherCNIC = model.FatherCNIC,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                MaritalStatus = model.MaritalStatus,
                Religion = model.Religion,
                Caste = model.Caste,
                Sect = model.Sect,
                ContactNo = model.ContactNo,
                Email = model.Email,
                PECNumber = model.PECNumber,
                CandidateType = model.CandidateType??"Candidate",
                PresentAddress = model.PresentAddress,
                Accommodation=model.Accommodation,
                PermanentAddress = model.PermanentAddress
            },

            // Map 1:1 Family Summary
            FamilySummary = new ApplicantFamilySummary
            {
                BrothersTotal = model.BrothersTotal,
                BrothersMarried = model.BrothersMarried,
                BrothersUnmarried = model.BrothersUnmarried,
                SistersTotal = model.SistersTotal,
                SistersMarried = model.SistersMarried,
                SistersUnmarried = model.SistersUnmarried,
                ChildrenTotal = model.ChildrenTotal,
                ChildrenMarried = model.ChildrenMarried,
                ChildrenUnmarried = model.ChildrenUnmarried
            },

            // Map 1:1 Financial Detail
            FinancialDetail = new ApplicantFinancialDetail
            {
                CurrentSalary = model.CurrentSalary,
                ExpectedSalary = model.ExpectedSalary,
                OtherBenefits = model.OtherBenefits,
                OtherFacilities=model.OtherFacilities,

                FamilyIncomeDetail = model.FamilyIncomeDetail
            },

            // Map 1:1 Military Detail
            MilitaryDetail = new ApplicantMilitaryDetail
            {
                ArmyNumber = model.ArmyNumber,
                ArmyUnit = model.ArmyUnit,
                ArmyCharacter = model.ArmyCharacter,
                ArmyPayScale = model.ArmyPayScale
            },
   // Map 1:N Collections (Mapster can handle these because the names match)
            Educations = model.Educations.Adapt<List<ApplicantEducation>>(),
            Experiences = model.Experiences.Adapt<List<ApplicantExperience>>(),
            Siblings = model.Siblings.Adapt<List<ApplicantSibling>>(),
            InternalRelatives = model.InternalRelatives.Adapt<List<ApplicantInternalRelative>>(),
            Skills = model.Skills.Adapt<List<ApplicantSkill>>(),
            Certifications = model.Certifications.Adapt<List<ApplicantCertification>>(),
            Achievements = model.Achievements.Adapt<List<ApplicantAchievement>>()
            // Map 1:N Collections (Mapster can handle these because the names match)
            // Educations = model.Educations.Adapt<List<ApplicantEducation>>(),
            // Experiences = model.Experiences.Adapt<List<ApplicantExperience>>(),
            // Siblings = model.Siblings.Adapt<List<ApplicantSibling>>(),
            // InternalRelatives = model.InternalRelatives.Adapt<List<ApplicantInternalRelative>>(),
            // Skills = model.Skills.Adapt<List<ApplicantSkill>>(),
            // Certifications = model.Certifications.Adapt<List<ApplicantCertification>>(),
            // Achievements = model.Achievements.Adapt<List<ApplicantAchievement>>()
        };
applicant.Id = applicantId;
applicant.Documents = documentEntities;
        // 4. Dispatch Command
        var command = new SubmitApplicationCommand(applicant, model.JobId);
        await _commandProcessor.SendAsync(command);

        return RedirectToAction("Success", new { code = command.GeneratedTrackingCode });
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", "Server Error: " + ex.Message);
        return View("Apply", model);
    }
}
    [HttpGet]
    public IActionResult Success(string code)
    {
        return View(model: code);
    }
}