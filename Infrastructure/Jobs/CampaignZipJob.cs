using System.IO.Compression;
using System.Text;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Career635.Infrastructure.Persistence;
using Career635.Domain.Entities.Jobs;
using Quartz;

namespace Career635.Infrastructure.Jobs;

public class CampaignZipJob(
    AppDbContext context, 
    IConfiguration config, 
    IWebHostEnvironment env,
    ILogger<CampaignZipJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext contextJob)
    {
        var taskIdStr = contextJob.MergedJobDataMap.GetString("TaskId");
        if (!Guid.TryParse(taskIdStr, out var taskId)) return;

        var task = await context.Set<CampaignExportTask>()
            .Include(t => t.Campaign)
            .FirstOrDefaultAsync(x => x.Id == taskId);
            
        if (task == null) return;

        try
        {
            task.Status = "Processing";
            await context.SaveChangesAsync();

            // 1. Resolve Paths
            string root = config.GetValue<string>("StorageSettings:DocumentRoot") ?? "uploads";
            string baseDir = root.StartsWith("wwwroot") 
                ? Path.Combine(env.ContentRootPath, root) 
                : root;

            // This is the source folder containing all JobId/CNIC subfolders
            string campaignSourcePath = Path.Combine(baseDir, task.CampaignId.ToString());
            
            // This is where the .zip file lives
            string exportDir = Path.Combine(baseDir, "exports");
            if (!Directory.Exists(exportDir)) Directory.CreateDirectory(exportDir);

            // Ensure the campaign source folder exists (even if empty) so we can place the Excel there
            if (!Directory.Exists(campaignSourcePath)) Directory.CreateDirectory(campaignSourcePath);

            // 2. Generate the Excel Report inside the Campaign Folder
            string excelName = $"Master_Registry_{task.Campaign.CampaignCode}.xlsx";
            string excelPath = Path.Combine(campaignSourcePath, excelName);
            await GenerateMasterExcelAsync(task.CampaignId, excelPath);

            // 3. Prepare ZIP Path
            string zipFileName = $"{task.Campaign.CampaignCode}_Personnel_Dossier_{DateTime.Now:yyyyMMdd}.zip";
            string finalZipPath = Path.Combine(exportDir, zipFileName);

            // 4. Delete previous ZIP if exists (1 zip per user/campaign requirement)
            if (File.Exists(finalZipPath)) File.Delete(finalZipPath);

            // 5. ZIP the entire Campaign folder (Includes: Excel + Subfolders with Photos/CVs)
            ZipFile.CreateFromDirectory(campaignSourcePath, finalZipPath);

            // 6. Finalize Task
            task.Status = "Completed";
            task.DownloadUrl = $"/uploads/exports/{zipFileName}";
            task.ProcessedAt = DateTime.UtcNow;
            task.ErrorMessage = null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to generate ZIP for Campaign {CampId}", task.CampaignId);
            task.Status = "Failed";
            task.ErrorMessage = ex.Message;
        }

        await context.SaveChangesAsync();
    }
    private async Task GenerateMasterExcelAsync(Guid campaignId, string filePath)
    {
        // Fetch ALL data with exhaustive Includes
        var applications = await context.JobApplications
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
            .Include(a => a.Applicant).ThenInclude(x => x.Skills)
            .Include(a => a.Applicant).ThenInclude(x => x.Certifications)
            .Include(a => a.Applicant).ThenInclude(x => x.Achievements)
            .Where(a => a.JobOpening.CampaignId == campaignId)
            .ToListAsync();

        using var workbook = new XLWorkbook();

        // --- SHEET 1: MASTER BIO-DATA (EXHAUSTIVE) ---
        var wsMaster = workbook.Worksheets.Add("Master Registry");
        var headers = new[] { 
            "Tracking ID", "Job Applied For", "Current Status", "Applied At",
            "Full Name", "CNIC Number", "Father Name", "Father CNIC", "DOB", "Gender", 
            "Marital Status", "Religion", "Caste", "Sect", "Contact No", "Email", "PEC No",
            "Present Address", "Permanent Address", "Army No", "Army Unit", "Army Character", "Army Scale",
            "Current Salary", "Expected Salary", "Family Income Detail", "Total Brothers", "Total Sisters", "Total Children", "CandiaiteType", "Accommodation","SistersMarried","BrothersMarried","ChildrenMarried","SistersUnMarried","BrothersUnMarried","ChildrenUnMarried"
        };
        for (int i = 0; i < headers.Length; i++) wsMaster.Cell(1, i + 1).Value = headers[i];

        for (int i = 0; i < applications.Count; i++)
        {
            var app = applications[i];
            var r = i + 2;
            var a = app.Applicant;
            var p = a.PersonalInfo;
            var m = a.MilitaryDetail;
            var f = a.FinancialDetail;
            var fs = a.FamilySummary;

            wsMaster.Cell(r, 1).Value = a.TrackingCode;
            wsMaster.Cell(r, 2).Value = app.JobOpening.Title;
            wsMaster.Cell(r, 3).Value = app.Status;
            wsMaster.Cell(r, 4).Value = app.AppliedAt.DateTime;
            wsMaster.Cell(r, 5).Value = a.FullName;
            wsMaster.Cell(r, 6).Value = a.CNICNumber;
            wsMaster.Cell(r, 7).Value = p?.FatherName;
            wsMaster.Cell(r, 8).Value = p?.FatherCNIC;
            wsMaster.Cell(r, 9).Value = p?.DateOfBirth;
            wsMaster.Cell(r, 10).Value = p?.Gender;
            wsMaster.Cell(r, 11).Value = p?.MaritalStatus;
            wsMaster.Cell(r, 12).Value = p?.Religion;
            wsMaster.Cell(r, 13).Value = p?.Caste;
            wsMaster.Cell(r, 14).Value = p?.Sect;
            wsMaster.Cell(r, 15).Value = p?.ContactNo;
            wsMaster.Cell(r, 16).Value = p?.Email;
            wsMaster.Cell(r, 17).Value = p?.PECNumber;
            wsMaster.Cell(r, 18).Value = p?.PresentAddress;
            wsMaster.Cell(r, 19).Value = p?.PermanentAddress;
            wsMaster.Cell(r, 20).Value = m?.ArmyNumber;
            wsMaster.Cell(r, 21).Value = m?.ArmyUnit;
            wsMaster.Cell(r, 22).Value = m?.ArmyCharacter;
            wsMaster.Cell(r, 23).Value = m?.ArmyPayScale;
            wsMaster.Cell(r, 24).Value = f?.CurrentSalary +"\nBenefits: "+ f?.OtherBenefits +"\n Facitlites "+f?.OtherFacilities;
            wsMaster.Cell(r, 25).Value = f?.ExpectedSalary;
            wsMaster.Cell(r, 26).Value = f?.FamilyIncomeDetail;
           
            
            wsMaster.Cell(r, 27).Value = fs?.BrothersTotal;
            wsMaster.Cell(r, 28).Value = fs?.SistersTotal;
            wsMaster.Cell(r, 29).Value = fs?.ChildrenTotal;
            wsMaster.Cell(r, 30).Value = p?.CandidateType;
             wsMaster.Cell(r, 31).Value = p?.Accommodation;
          
            wsMaster.Cell(r, 32).Value = fs?.SistersMarried;
            wsMaster.Cell(r, 33).Value = fs?.BrothersMarried;
            wsMaster.Cell(r, 34).Value = fs?.ChildrenMarried;

            wsMaster.Cell(r, 35).Value = fs?.SistersUnmarried; //SistersMarried;
            wsMaster.Cell(r, 36).Value = fs?.BrothersUnmarried;
            wsMaster.Cell(r, 37).Value = fs?.ChildrenUnmarried;


        
        }

        // --- SHEET 2: EDUCATION ---
        var wsEdu = workbook.Worksheets.Add("Education");
        wsEdu.Cell(1, 1).Value = "CNIC"; wsEdu.Cell(1, 2).Value = "Level"; wsEdu.Cell(1, 3).Value = "Qualification"; wsEdu.Cell(1, 4).Value = "Institution"; wsEdu.Cell(1, 5).Value = "Result";
        int eduRow = 2;
        foreach(var app in applications) {
            foreach(var e in app.Applicant.Educations) {
                wsEdu.Cell(eduRow, 1).Value = app.Applicant.CNICNumber;
                wsEdu.Cell(eduRow, 3).Value = e.Qualification;
                wsEdu.Cell(eduRow, 4).Value = e.BoardUniversity;
                wsEdu.Cell(eduRow, 5).Value = e.CgpaPercentage;
                eduRow++;
            }
        }

        // --- SHEET 3: EXPERIENCE ---
        var wsExp = workbook.Worksheets.Add("Experience");
        wsExp.Cell(1, 1).Value = "CNIC"; wsExp.Cell(1, 2).Value = "Organization"; wsExp.Cell(1, 3).Value = "Designation"; wsExp.Cell(1, 4).Value = "From"; wsExp.Cell(1, 5).Value = "To";
        int expRow = 2;
        foreach(var app in applications) {
            foreach(var ex in app.Applicant.Experiences) {
                wsExp.Cell(expRow, 1).Value = app.Applicant.CNICNumber;
                wsExp.Cell(expRow, 2).Value = ex.OrganizationName;
                wsExp.Cell(expRow, 3).Value = ex.Designation;
                wsExp.Cell(expRow, 4).Value = ex.FromDate;
                wsExp.Cell(expRow, 5).Value = ex.ToDate;
                wsExp.Cell(expRow, 6).Value = ex.KeyResponsibilities;
                expRow++;
            }
        }

        // --- SHEET 4: SIBLINGS ---
        var wsSib = workbook.Worksheets.Add("Siblings");
        wsSib.Cell(1, 1).Value = "Applicant CNIC"; wsSib.Cell(1, 2).Value = "Sibling Name"; wsSib.Cell(1, 3).Value = "Gender"; wsSib.Cell(1, 4).Value = "Occupation";
        int sibRow = 2;
        foreach(var app in applications) {
            foreach(var s in app.Applicant.Siblings) {
                wsSib.Cell(sibRow, 1).Value = app.Applicant.CNICNumber;
                wsSib.Cell(sibRow, 2).Value = s.Name;
                wsSib.Cell(sibRow, 3).Value = s.Gender;
                wsSib.Cell(sibRow, 4).Value = s.Occupation;
                wsSib.Cell(sibRow, 5).Value = s.CNIC;
                wsSib.Cell(sibRow, 6).Value = s.DateOfBirth;
                wsSib.Cell(sibRow, 7).Value = s.Designation;
                wsSib.Cell(sibRow, 8).Value = s.Organization;

                sibRow++;
            }
        }

        // --- SHEET 5: PROFESSIONAL EXTRAS (Skills/Certs) ---
        var wsExtra = workbook.Worksheets.Add("Skills & Certs");
        wsExtra.Cell(1, 1).Value = "CNIC"; wsExtra.Cell(1, 2).Value = "Type"; wsExtra.Cell(1, 3).Value = "Name"; wsExtra.Cell(1, 4).Value = "Detail";
        int exRow = 2;
        foreach(var app in applications) {
            foreach(var sk in app.Applicant.Skills) {
                wsExtra.Cell(exRow, 1).Value = app.Applicant.CNICNumber; wsExtra.Cell(exRow, 2).Value = "Skill"; wsExtra.Cell(exRow, 3).Value = sk.SkillName; wsExtra.Cell(exRow, 4).Value = sk.Proficiency;
                exRow++;
            }
            foreach(var ct in app.Applicant.Certifications) {
                wsExtra.Cell(exRow, 1).Value = app.Applicant.CNICNumber; wsExtra.Cell(exRow, 2).Value = "Certification"; wsExtra.Cell(exRow, 3).Value = ct.CertificateName; wsExtra.Cell(exRow, 4).Value = ct.IssuingBody;
                exRow++;
            }
        }

        // APPLY CORPORATE STYLING
        foreach (var ws in workbook.Worksheets)
        {
            var range = ws.Range(1, 1, 1, ws.ColumnsUsed().Count());
            range.Style.Fill.BackgroundColor = XLColor.FromHtml("#064E3B"); // Emerald 950
            range.Style.Font.FontColor = XLColor.White;
            range.Style.Font.Bold = true;
            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(1);
        }

        workbook.SaveAs(filePath);
    }
}