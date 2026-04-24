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
//   public async Task Execute(IJobExecutionContext contextJob)
//     {
//         var taskIdStr = contextJob.MergedJobDataMap.GetString("TaskId");
//         if (!Guid.TryParse(taskIdStr, out var taskId)) return;

//         var task = await context.Set<CampaignExportTask>().Include(t => t.Campaign).FirstOrDefaultAsync(x => x.Id == taskId);
//         if (task == null) return;

//         // Create a unique temporary working directory for this specific export
//         string tempRoot = Path.Combine(env.ContentRootPath, "temp_exports", taskId.ToString());

//         try
//         {
//             task.Status = "Processing";
//             await context.SaveChangesAsync();

//             if (Directory.Exists(tempRoot)) Directory.Delete(tempRoot, true);
//             Directory.CreateDirectory(tempRoot);


//             // 1. Fetch ALL data (including every document link)
//             var applications = await context.JobApplications
//                 .AsNoTracking()
//                 .Include(a => a.JobOpening)
//                 .Include(a => a.Applicant).ThenInclude(x => x.PersonalInfo)
//             .Include(a => a.Applicant).ThenInclude(x => x.FamilySummary)
//             .Include(a => a.Applicant).ThenInclude(x => x.FinancialDetail)
//             .Include(a => a.Applicant).ThenInclude(x => x.MilitaryDetail)
//             .Include(a => a.Applicant).ThenInclude(x => x.Educations)
//             .Include(a => a.Applicant).ThenInclude(x => x.Experiences)
//             .Include(a => a.Applicant).ThenInclude(x => x.Siblings)
//             .Include(a => a.Applicant).ThenInclude(x => x.InternalRelatives)
//             .Include(a => a.Applicant).ThenInclude(x => x.Skills)
//             .Include(a => a.Applicant).ThenInclude(x => x.Certifications)
//             .Include(a => a.Applicant).ThenInclude(x => x.Achievements)
//                 .Include(a => a.Applicant).ThenInclude(x => x.Documents) // CRITICAL
//                 .Where(a => a.JobOpening.CampaignId == task.CampaignId)
//                 .ToListAsync();

//             // 2. Resolve Base Path (Consistent with FileStorageService)
//             string storageRoot = config.GetValue<string>("StorageSettings:DocumentRoot") ?? "uploads";
//             string baseStoragePath = storageRoot.StartsWith("wwwroot") 
//                 ? Path.Combine(env.ContentRootPath, storageRoot) 
//                 : storageRoot;

//             // 3. PHYSICALLY GATHER FILES
//             foreach (var app in applications)
//             {
//                 // Create a subfolder in TEMP for each applicant: [FullName]_[CNIC]
//                 string applicantFolder = Path.Combine(tempRoot, $"{app.Applicant.FullName}_{app.Applicant.CNICNumber}".Replace(" ", "_"));
//                 Directory.CreateDirectory(applicantFolder);

//                 // A. Copy Passport Image
//                 await SafeCopyFile(baseStoragePath, app.Applicant.PassportImageUrl, applicantFolder, "Passport_Photo");

//                 // B. Copy CV
//                 await SafeCopyFile(baseStoragePath, app.Applicant.CvUrl, applicantFolder, "Main_CV");

//                 // C. Copy All Other Documents (Degrees, Certs, etc.)
//                 foreach (var doc in app.Applicant.Documents)
//                 {
//                     await SafeCopyFile(baseStoragePath, doc.FileUrl, applicantFolder, doc.DocumentType);
//                 }
//             }

//             // 4. GENERATE EXCEL INSIDE THE TEMP ROOT
//             string excelName = $"Master_Registry_{task.Campaign?.CampaignCode ?? "Default"}.xlsx";
//             string excelPath = Path.Combine(tempRoot, excelName);
//             await GenerateMasterExcelAsync(applications, excelPath);

//             // 5. PREPARE FINAL ZIP
//             string exportDir = Path.Combine(baseStoragePath, "exports");
//             if (!Directory.Exists(exportDir)) Directory.CreateDirectory(exportDir);

//             string zipFileName = $"{task.Campaign?.CampaignCode ?? "Default"}_Personnel_Dossier_{DateTime.Now:yyyyMMdd_HHmm}.zip";
//             string finalZipPath = Path.Combine(exportDir, zipFileName);

//             if (File.Exists(finalZipPath)) File.Delete(finalZipPath);

//             // Zip the temporary folder (now contains organized subfolders + Excel)
//             ZipFile.CreateFromDirectory(tempRoot, finalZipPath);

//             // 6. FINALIZE TASK
//             task.Status = "Completed";
//             task.DownloadUrl = $"/uploads/exports/{zipFileName}";
//             task.ProcessedAt = DateTime.UtcNow;
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, "Campaign Export Failed for Task {TaskId}", taskId);
//             task.Status = "Failed";
//             task.ErrorMessage = ex.Message;
//         }
//         finally
//         {
//             // Cleanup: Delete the temporary working directory
//             if (Directory.Exists(tempRoot)) Directory.Delete(tempRoot, true);
//         }

//         await context.SaveChangesAsync();
//     }

 public async Task Execute(IJobExecutionContext contextJob)
    {
        var taskIdStr = contextJob.MergedJobDataMap.GetString("TaskId");
        if (!Guid.TryParse(taskIdStr, out var taskId)) return;

        var task = await context.Set<CampaignExportTask>()
            .Include(t => t.Campaign)
            .FirstOrDefaultAsync(x => x.Id == taskId);
            
        if (task == null) return;

        // Unique temp workspace for this specific export
        string tempRoot = Path.Combine(env.ContentRootPath, "temp_exports", taskId.ToString());

        try
        {
            task.Status = "Processing";
            await context.SaveChangesAsync();

            if (Directory.Exists(tempRoot)) Directory.Delete(tempRoot, true);
            Directory.CreateDirectory(tempRoot);
  var jobsInCampaign = await context.JobOpenings
                .AsNoTracking()
                .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.PersonalInfo)
                           .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.FamilySummary)

            .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.FinancialDetail)
            .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.MilitaryDetail)
            .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.Educations)
            .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.Experiences)
            .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.Siblings)
            .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.InternalRelatives)
            .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.Skills)
            .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.Certifications)
            .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.Achievements)
            .Include(j => j.JobApplications).ThenInclude(a => a.Applicant).ThenInclude(x => x.Documents)
                .Where(j => j.CampaignId == task.CampaignId)
                .ToListAsync();

            // 2. Resolve Server Storage Root
            string storageRoot = config.GetValue<string>("StorageSettings:DocumentRoot") ?? "uploads";
            string baseStoragePath = storageRoot.StartsWith("wwwroot") 
                ? Path.Combine(env.ContentRootPath, storageRoot) 
                : storageRoot;

            // 3. ORGANIZE PHYSICAL FILES INTO TEMP STRUCTURE
            foreach (var job in jobsInCampaign)
            {
                // Folder 1: Job Title [JobId]
                string jobFolderName = Sanitize($"{job.Title}_{job.Id.ToString()[..8]}");
                string jobPath = Path.Combine(tempRoot, jobFolderName);
                Directory.CreateDirectory(jobPath);

                foreach (var app in job.JobApplications)
                {
                    // Folder 2: Applicant Name [CNIC]
                    string applicantFolderName = Sanitize($"{app.Applicant.FullName}_{app.Applicant.CNICNumber}");
                    string applicantPath = Path.Combine(jobPath, applicantFolderName);
                    
                    // Folder 3: Categorized Subfolders
                    string profilePath = Path.Combine(applicantPath, "Profile_Photo");
                    string cvPath = Path.Combine(applicantPath, "CV_Portfolio");
                    string docsPath = Path.Combine(applicantPath, "Supporting_Documents");

                    Directory.CreateDirectory(profilePath);
                    Directory.CreateDirectory(cvPath);
                    Directory.CreateDirectory(docsPath);

                    // A. Copy Passport Image
                    await SafeCopy(baseStoragePath, app.Applicant.PassportImageUrl, profilePath, "Passport_Photo");

                    // B. Copy Main CV
                    await SafeCopy(baseStoragePath, app.Applicant.CvUrl, cvPath, "Main_CV");

                    // C. Copy All Other Documents (Grouped by DocumentType)
                    foreach (var doc in app.Applicant.Documents)
                    {
                        string typeFolder = Path.Combine(docsPath, Sanitize(doc.DocumentType));
                        if (!Directory.Exists(typeFolder)) Directory.CreateDirectory(typeFolder);
                        await SafeCopy(baseStoragePath, doc.FileUrl, typeFolder, "Document");
                    }
                }
            }

            // 4. GENERATE EXCEL (Placed at the root of the ZIP)
            string excelName = $"Campaign_Registry_{task.Campaign?.CampaignCode ?? "Default"}.xlsx";
            await GenerateMasterExcelAsync(jobsInCampaign.SelectMany(x => x.JobApplications).ToList(), Path.Combine(tempRoot, excelName));

            // 5. FINALIZE ZIP
            string exportDir = Path.Combine(baseStoragePath, "exports");
            if (!Directory.Exists(exportDir)) Directory.CreateDirectory(exportDir);

            string zipFileName = $"{task.Campaign?.CampaignCode ?? "Default"}_Full_Dossier.zip";
            string finalZipPath = Path.Combine(exportDir, zipFileName);

            if (File.Exists(finalZipPath)) File.Delete(finalZipPath);
            ZipFile.CreateFromDirectory(tempRoot, finalZipPath);

            task.Status = "Completed";
            task.DownloadUrl = $"/uploads/exports/{zipFileName}";
            task.ProcessedAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Export Error on Task {TaskId}", taskId);
            task.Status = "Failed";
            task.ErrorMessage = ex.Message;
        }
        finally
        {
            if (Directory.Exists(tempRoot)) Directory.Delete(tempRoot, true);
        }

        await context.SaveChangesAsync();
    }

    private async Task SafeCopy(string baseRoot, string? relativePath, string targetDir, string fileNamePrefix)
    {
        if (string.IsNullOrEmpty(relativePath)) return;
        
        // Clean relative path of leading slashes
        string cleanRelative = relativePath.Replace("\\", "/").TrimStart('/');
        string source = Path.GetFullPath(Path.Combine(baseRoot, cleanRelative));

        if (File.Exists(source))
        {
            string ext = Path.GetExtension(source);
            string dest = Path.Combine(targetDir, $"{fileNamePrefix}{ext}");
            
            // Handle multiple files of same type (e.g. 2 degrees)
            int count = 1;
            while(File.Exists(dest)) dest = Path.Combine(targetDir, $"{fileNamePrefix}_{count++}{ext}");
            
            await Task.Run(() => File.Copy(source, dest, true));
        }
    }

    private string Sanitize(string name)
    {
        // Remove invalid folder characters
        char[] invalid = Path.GetInvalidFileNameChars();
        foreach (char c in invalid) name = name.Replace(c, '_');
        return name.Replace(" ", "_");
    }
    private async Task GenerateMasterExcelAsync(List<JobApplication>  applications, string filePath)
    {
        // Fetch ALL data with exhaustive Includes
      

        using var workbook = new XLWorkbook();

        // --- SHEET 1: MASTER BIO-DATA (EXHAUSTIVE) ---
        var wsMaster = workbook.Worksheets.Add("Master Registry");
       
       
       
        var headers = new[] { 
            "Tracking ID", 
            "Job Applied For", 
            "Current Status", "Applied At",
            "Full Name", "CNIC Number", "Father Name", "Father CNIC", "DOB", "Gender", 
            "Marital Status", "Religion", "Caste", "Sect", "Contact No", "Email", "PEC No",
            "Present Address", "Permanent Address", "Army No", "Army Unit", "Army Character", "Army Scale",
            "Current Salary", "Expected Salary", "Family Income Detail", "Total Brothers", "Total Sisters", "Total Children", "CandiaiteType", "Accommodation","SistersMarried","BrothersMarried","ChildrenMarried","SistersUnMarried","BrothersUnMarried","ChildrenUnMarried","JobId","JobName","CV","Photo"
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
            wsMaster.Cell(r, 24).Value = f?.CurrentSalary +",\nBenefits: "+ f?.OtherBenefits +",\nFacitlites "+f?.OtherFacilities;
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
            wsMaster.Cell(r, 38).Value = app.JobOpeningId.ToString();
            wsMaster.Cell(r, 39).Value = app.JobOpening.Title.ToString();
            wsMaster.Cell(r, 40).Value = app.Applicant?.CvUrl?.ToString()??"N/A";
            wsMaster.Cell(r, 41).Value = app.Applicant?.PassportImageUrl?.ToString()??"N/A";


        
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
        wsSib.Cell(1, 1).Value = "Applicant CNIC"; 
        wsSib.Cell(1, 2).Value = "Sibling Name"; 
        wsSib.Cell(1, 3).Value = "Gender"; 
        wsSib.Cell(1, 4).Value = "Occupation";
        wsSib.Cell(1, 5).Value = "CNIC";
        wsSib.Cell(1, 6).Value = "DateOfBirth";
        wsSib.Cell(1, 7).Value = "Designation";
        wsSib.Cell(1, 8).Value = "Organization";

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


   var wsDoc = workbook.Worksheets.Add("Documents");
        wsDoc.Cell(1, 1).Value = "CNIC"; wsDoc.Cell(1, 2).Value = "DocType";
        wsDoc.Cell(1, 3).Value = "Id"; wsDoc.Cell(1, 4).Value = "Path";
        int wsdocRow = 2;
        foreach(var app in applications) {
            foreach(var ex in app.Applicant.Documents) {
                wsDoc.Cell(wsdocRow, 1).Value = app.Applicant.CNICNumber;
                wsDoc.Cell(wsdocRow, 2).Value = ex.DocumentType;
                wsDoc.Cell(wsdocRow, 3).Value = ex.Id.ToString();
                wsDoc.Cell(wsdocRow, 4).Value = ex.FileUrl;
            
        
                wsdocRow++;
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