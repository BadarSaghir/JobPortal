

using System.IO.Compression;
using System.Text;
using SpreadCheetah;
using SpreadCheetah.Styling;
using Microsoft.EntityFrameworkCore;
using Career635.Infrastructure.Persistence;
using Career635.Domain.Entities.Jobs;
using Career635.Domain.Entities.Applicants;
using Quartz;
using System.Drawing;

namespace Career635.Infrastructure.Jobs;

public class CampaignZipJob(
    AppDbContext context, 
    IConfiguration config, 
    IWebHostEnvironment env,
    ILogger<CampaignZipJob> logger) : IJob
{
    private string _logFilePath = string.Empty;

    public async Task Execute(IJobExecutionContext contextJob)
    {
        var taskIdStr = contextJob.MergedJobDataMap.GetString("TaskId");
        if (!Guid.TryParse(taskIdStr, out var taskId)) return;

        var task = await context.Set<CampaignExportTask>()
            .Include(t => t.Campaign)
            .FirstOrDefaultAsync(x => x.Id == taskId);

        if (task == null) return;

        // 1. Setup Paths
        string storageRoot = config.GetValue<string>("StorageSettings:DocumentRoot") ?? "uploads";
        string baseStoragePath = storageRoot.StartsWith("wwwroot") 
            ? Path.Combine(env.ContentRootPath, storageRoot) 
            : storageRoot;

        string tempRoot = Path.Combine(baseStoragePath, "temp_exports", taskId.ToString());
        string excelPath = Path.Combine(tempRoot, $"Campaign_Registry_{task.Campaign?.CampaignCode ?? "Default"}.xlsx");
        
        // Setup Logging File
        string logDir = Path.Combine(baseStoragePath, "logs", "exports");
        Directory.CreateDirectory(logDir);
        _logFilePath = Path.Combine(logDir, $"Log_{taskId}.txt");

        try
        {
            await Log("JOB STARTED", $"Task ID: {taskId} | Campaign: {task.Campaign?.Name}");
            await UpdateTaskStatus(task, "Initializing Workspace");

            if (Directory.Exists(tempRoot)) Directory.Delete(tempRoot, true);
            Directory.CreateDirectory(tempRoot);

            // 2. Excel Generation (Streaming via Projection)
            await Log("EXCEL", "Starting SpreadCheetah Streaming with DTOs...");
            await GenerateExcelAsync(task, excelPath);
            await Log("EXCEL", "SpreadCheetah finished successfully.");

            // 3. File Copying (Batching + Semaphore Strategy)
            await Log("FILES", "Starting concurrent physical file collection...");
            await ProcessFileCopiesAsync(task, tempRoot, baseStoragePath);
            await Log("FILES", "File collection finished.");

            // 4. Include Log file in the export folder before zipping
            string internalLogPath = Path.Combine(tempRoot, "Export_Process_Log.txt");
            File.Copy(_logFilePath, internalLogPath, true);

            // 5. Create ZIP
            await UpdateTaskStatus(task, "Compressing into ZIP...");
            string exportDir = Path.Combine(baseStoragePath, "exports");
            Directory.CreateDirectory(exportDir);

            string zipFileName = $"{task.Campaign?.CampaignCode ?? "Default"}_Export.zip";
        
            string finalZipPath = Path.Combine(exportDir, zipFileName);
            if(File.Exists(finalZipPath)) File.Delete(finalZipPath);
            
            ZipFile.CreateFromDirectory(tempRoot, finalZipPath, CompressionLevel.Optimal, false);
            await Log("ZIP", $"Zip created at: {finalZipPath}");

            // 6. Success
            task.Status = "Completed";
            task.DownloadUrl = $"/uploads/exports/{zipFileName}";
            task.ErrorMessage = null;
        }
        catch (Exception ex)
        {
            await Log("CRITICAL ERROR", $"{ex.Message}\n{ex.StackTrace}");
            task.Status = "Failed";
            task.ErrorMessage = ex.Message;
        }
        finally
        {
            task.ProcessedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            if (Directory.Exists(tempRoot)) try { Directory.Delete(tempRoot, true); } catch { }
            await Log("JOB FINISHED", "Cleanup complete.");
        }
    }

    private async Task GenerateExcelAsync(CampaignExportTask task, string excelPath)
    {
        await using var stream = File.Create(excelPath);
        using var spreadsheet = await Spreadsheet.CreateNewAsync(stream);

        var hStyle = spreadsheet.AddStyle(new Style { 
            Font = { Bold = true, Color = Color.White }, 
            Fill = { Color = Color.Green } 
        });

        await spreadsheet.StartWorksheetAsync("Master Registry");
        await WriteMasterSheet(spreadsheet, hStyle, task.CampaignId);

        await spreadsheet.StartWorksheetAsync("Education");
        await WriteEducationSheet(spreadsheet, hStyle, task.CampaignId);

        await spreadsheet.StartWorksheetAsync("Experience");
        await WriteExperienceSheet(spreadsheet, hStyle, task.CampaignId);

        await spreadsheet.StartWorksheetAsync("Siblings");
        await WriteSiblingSheet(spreadsheet, hStyle, task.CampaignId);

        await spreadsheet.StartWorksheetAsync("Skills_Certs");
        await WriteSkillsCertsSheet(spreadsheet, hStyle, task.CampaignId);

        await spreadsheet.StartWorksheetAsync("Relatives");
        await WriteRelativeSheet(spreadsheet, hStyle, task.CampaignId);

        await spreadsheet.StartWorksheetAsync("Documents");
        await WriteDocumentsSheet(spreadsheet, hStyle, task.CampaignId);

        await spreadsheet.StartWorksheetAsync("Achievements");
        await WriteAchievementSheet(spreadsheet, hStyle, task.CampaignId);

        await spreadsheet.FinishAsync();
    }

    #region Database Writers (Streaming Projection)

private async Task WriteMasterSheet(Spreadsheet s, StyleId h, Guid? campaignId)
    {
        // Exact 44 headers from your original code
        string[] headers = 
        { 
            "Tracking ID", "Job Applied For", "Current Status", "Applied At",
            "Full Name", "CNIC Number", "Father Name", "Father CNIC", "DOB", "Gender", 
            "Marital Status", "Religion", "Caste", "Sect", "Contact No", "Email", "PEC No",
            "Present Address", "Permanent Address", "Army No", "Army Unit", "Army Character", 
            "Army Scale", "Current Salary", "Other Benefits", "Other Facilities", "Expected Salary", 
            "Family Income Detail", "Total Brothers", "Total Sisters", "Total Children", 
            "Candidate Type", "Accommodation", "Sisters Married", "Brothers Married", 
            "Children Married", "Sisters Unmarried", "Brothers Unmarried", "Children Unmarried",
            "JobId", "JobName", "CV Path", "Photo Path", "ApplicantId"
        };
        
        await s.AddRowAsync(headers.Select(x => new StyledCell(x, h)).ToList());

        // PROJECTION: Pulls only the required columns from the DB into a flat object.
        // This prevents EF Core from tracking massive nested object graphs in memory.
        var query = context.JobApplications.AsNoTracking()
            .Where(a => a.JobOpening.CampaignId == campaignId)
            .Select(a => new
            {
                // App & Job
                TrackingId = a.Applicant.TrackingCode,
                JobAppliedFor = a.JobOpening.Title,
                Status = a.Status,
                AppliedAt = a.AppliedAt,
                JobId = a.JobOpeningId,
                ApplicantId = a.ApplicantId,

                // Applicant Base
                FullName = a.Applicant.FullName,
                CNIC = a.Applicant.CNICNumber,
                CvUrl = a.Applicant.CvUrl,
                PhotoUrl = a.Applicant.PassportImageUrl,

                DOB = a.Applicant.PersonalInfo.DateOfBirth,
                // Personal Info
                FatherName = a.Applicant.PersonalInfo!.FatherName,
                FatherCNIC = a.Applicant.PersonalInfo.FatherCNIC,
                Gender = a.Applicant.PersonalInfo.Gender,
                MaritalStatus = a.Applicant.PersonalInfo.MaritalStatus,
                Religion = a.Applicant.PersonalInfo.Religion,
                Caste = a.Applicant.PersonalInfo.Caste,
                Sect = a.Applicant.PersonalInfo.Sect,
                ContactNo = a.Applicant.PersonalInfo.ContactNo,
                Email = a.Applicant.PersonalInfo.Email,
                PECNo = a.Applicant.PersonalInfo.PECNumber,
                PresentAddress = a.Applicant.PersonalInfo.PresentAddress,
                PermanentAddress = a.Applicant.PersonalInfo.PermanentAddress,
                CandidateType = a.Applicant.PersonalInfo.CandidateType,
                Accommodation = a.Applicant.PersonalInfo.Accommodation,

                // Military
                ArmyNo = a.Applicant.MilitaryDetail!.ArmyNumber,
                ArmyUnit = a.Applicant.MilitaryDetail.ArmyUnit,
                ArmyCharacter = a.Applicant.MilitaryDetail.ArmyCharacter,
                ArmyScale = a.Applicant.MilitaryDetail.ArmyPayScale,

                // Financial
                CurrentSalary = a.Applicant.FinancialDetail!.CurrentSalary,
                OtherBenefits = a.Applicant.FinancialDetail.OtherBenefits,
                OtherFacilities = a.Applicant.FinancialDetail.OtherFacilities,
                ExpectedSalary = a.Applicant.FinancialDetail.ExpectedSalary,
                FamilyIncomeDetail = a.Applicant.FinancialDetail.FamilyIncomeDetail,

                // Family Summary
                BrothersTotal = a.Applicant.FamilySummary!.BrothersTotal,
                SistersTotal = a.Applicant.FamilySummary.SistersTotal,
                ChildrenTotal = a.Applicant.FamilySummary.ChildrenTotal,
                SistersMarried = a.Applicant.FamilySummary.SistersMarried,
                BrothersMarried = a.Applicant.FamilySummary.BrothersMarried,
                ChildrenMarried = a.Applicant.FamilySummary.ChildrenMarried,
                SistersUnmarried = a.Applicant.FamilySummary.SistersUnmarried,
                BrothersUnmarried = a.Applicant.FamilySummary.BrothersUnmarried,
                ChildrenUnmarried = a.Applicant.FamilySummary.ChildrenUnmarried
            })
            .AsAsyncEnumerable();

        int count = 0;
        await foreach (var row in query)
        {
            // Map the 44 projected properties exactly to the 44 headers
            await s.AddRowAsync(new List<DataCell> 
            {
                new(row.TrackingId ?? "N/A"),
                new(row.JobAppliedFor ?? "N/A"),
                new(row.Status ?? "N/A"),
                new(row.AppliedAt.ToString("yyyy-MM-dd HH:mm")),
                
                new(row.FullName ?? "N/A"),
                new(row.CNIC ?? "N/A"),
                new(row.FatherName ?? "N/A"),
                new(row.FatherCNIC ?? "N/A"),
                new(row.DOB.ToString("yyyy-MM-dd") ?? "N/A"),
                new(row.Gender ?? "N/A"),
                
                new(row.MaritalStatus ?? "N/A"),
                new(row.Religion ?? "N/A"),
                new(row.Caste ?? "N/A"),
                new(row.Sect ?? "N/A"),
                new(row.ContactNo ?? "N/A"),
                new(row.Email ?? "N/A"),
                new(row.PECNo ?? "N/A"),
                
                new(row.PresentAddress ?? "N/A"),
                new(row.PermanentAddress ?? "N/A"),
                new(row.ArmyNo ?? "N/A"),
                new(row.ArmyUnit ?? "N/A"),
                new(row.ArmyCharacter ?? "N/A"),
                
                new(row.ArmyScale ?? "N/A"),
                new(row.CurrentSalary?.ToString() ?? "N/A"),
                new(row.OtherBenefits ?? "N/A"),
                new(row.OtherFacilities ?? "N/A"),
                new(row.ExpectedSalary?.ToString() ?? "N/A"),
                
                new(row.FamilyIncomeDetail ?? "N/A"),
                new(row.BrothersTotal.ToString() ?? "0"),
                new(row.SistersTotal.ToString() ?? "0"),
                new(row.ChildrenTotal.ToString() ?? "0"),
                
                new(row.CandidateType ?? "N/A"),
                new(row.Accommodation ?? "N/A"),
                new(row.SistersMarried.ToString() ?? "0"),
                new(row.BrothersMarried.ToString() ?? "0"),
                
                new(row.ChildrenMarried.ToString() ?? "0"),
                new(row.SistersUnmarried.ToString() ?? "0"),
                new(row.BrothersUnmarried.ToString() ?? "0"),
                new(row.ChildrenUnmarried.ToString() ?? "0"),
                
                new(row.JobId.ToString()),
                new(row.JobAppliedFor ?? "N/A"), // JobName (repeated from column 2)
                new(row.CvUrl ?? "N/A"),
                new(row.PhotoUrl ?? "N/A"),
                new(row.ApplicantId.ToString())
            });
            count++;
        }
        await Log("DATABASE", $"Master Sheet: {count} rows written.");
    }
    private async Task WriteEducationSheet(Spreadsheet s, StyleId h, Guid? campaignId)
    {
        string[] headers = { "CNIC", "Level","Major Field" ,"Qualification", "Institution", "Result","From","To" ,"ApplicantId" };
        await s.AddRowAsync(headers.Select(x => new StyledCell(x, h)).ToList());

        var query = context.Set<ApplicantEducation>().AsNoTracking().Include(x => x.Applicant).Include(x => x.DegreeLevel)
            .Where(x => context.JobApplications.Any(ja => ja.ApplicantId == x.ApplicantId && ja.JobOpening.CampaignId == campaignId))
            .AsAsyncEnumerable();

        await foreach (var x in query)
            await s.AddRowAsync(new List<DataCell> { new(x.Applicant.CNICNumber ?? ""), new(x.DegreeLevel?.Name ?? ""), new(x.MajorField ?? ""), new(x.Qualification ?? ""), new(x.BoardUniversity ?? ""), new(x.CgpaPercentage ?? ""), new(x.FromDate.ToString() ?? ""), new(x.ToDate.ToString() ?? ""), new(x.ApplicantId.ToString()) });
    }

    private async Task WriteExperienceSheet(Spreadsheet s, StyleId h, Guid? campaignId)
    {
        string[] headers = { "CNIC", "Organization", "Designation","Responsibilities", "From", "To", "ApplicantId" };
        await s.AddRowAsync(headers.Select(x => new StyledCell(x, h)).ToList());

        var query = context.Set<ApplicantExperience>().AsNoTracking().Include(x => x.Applicant)
            .Where(x => context.JobApplications.Any(ja => ja.ApplicantId == x.ApplicantId && ja.JobOpening.CampaignId == campaignId))
            .AsAsyncEnumerable();

        await foreach (var x in query)
            await s.AddRowAsync(new List<DataCell> { new(x.Applicant.CNICNumber ?? ""), new(x.OrganizationName ?? ""), new(x.Designation ?? ""),new(x.KeyResponsibilities ?? ""), new(x.FromDate.ToString("yyyy-MM-dd") ?? ""), new(x.ToDate?.ToString("yyyy-MM-dd") ?? ""), new(x.ApplicantId.ToString()) });
    }

    private async Task WriteSiblingSheet(Spreadsheet s, StyleId h, Guid? campaignId)
    {
        string[] headers = { "CNIC","Sibling CNIC", "Sibling Name", "Gender", "Occupation", "Organization","MaritalStatus","Designation","ApplicantId" };
        await s.AddRowAsync(headers.Select(x => new StyledCell(x, h)).ToList());

        var query = context.Set<ApplicantSibling>().AsNoTracking().Include(x => x.Applicant)
            .Where(x => context.JobApplications.Any(ja => ja.ApplicantId == x.ApplicantId && ja.JobOpening.CampaignId == campaignId)).AsAsyncEnumerable();

        await foreach (var x in query)
            await s.AddRowAsync(new List<DataCell> { new(x.Applicant.CNICNumber ?? ""), new(x.CNIC ?? ""), new(x.Name ?? ""), new(x.Gender ?? ""), new(x.Occupation ?? ""),new(x.MaritalStatus ?? ""),new(x.Organization ?? ""),new(x.Designation ?? ""), new(x.ApplicantId.ToString()) });
    }

    private async Task WriteSkillsCertsSheet(Spreadsheet s, StyleId h, Guid? campaignId)
    {
        string[] headers = { "CNIC", "Type", "Name", "Detail", "ApplicantId" };
        await s.AddRowAsync(headers.Select(x => new StyledCell(x, h)).ToList());

        var skills = context.Set<ApplicantSkill>().AsNoTracking()
            .Where(x => context.JobApplications.Any(ja => ja.ApplicantId == x.ApplicantId && ja.JobOpening.CampaignId == campaignId))
            .Select(x => new { x.Applicant.CNICNumber, Name = x.SkillName, Detail = x.Proficiency, x.ApplicantId })
            .AsAsyncEnumerable();
        
        await foreach (var x in skills) 
            await s.AddRowAsync(new List<DataCell> { new(x.CNICNumber ?? ""), new("Skill"), new(x.Name ?? ""), new(x.Detail ?? ""), new(x.ApplicantId.ToString()) });

        var certs = context.Set<ApplicantCertification>().AsNoTracking()
            .Where(x => context.JobApplications.Any(ja => ja.ApplicantId == x.ApplicantId && ja.JobOpening.CampaignId == campaignId))
            .Select(x => new { x.Applicant.CNICNumber, Name = x.CertificateName, Detail = x.IssuingBody, x.ApplicantId })
            .AsAsyncEnumerable();
            
        await foreach (var x in certs) 
            await s.AddRowAsync(new List<DataCell> { new(x.CNICNumber ?? ""), new("Certification"), new(x.Name ?? ""), new(x.Detail ?? ""), new(x.ApplicantId.ToString()) });
    }

    private async Task WriteRelativeSheet(Spreadsheet s, StyleId h, Guid? campaignId)
    {
        string[] headers = { "CNIC", "RelativeName", "Department", "Designation", "PayScale", "ApplicantId" };
        await s.AddRowAsync(headers.Select(x => new StyledCell(x, h)).ToList());

        var query = context.Set<ApplicantInternalRelative>().AsNoTracking()
            .Where(x => context.JobApplications.Any(ja => ja.ApplicantId == x.ApplicantId && ja.JobOpening.CampaignId == campaignId))
            .Select(x => new { x.Applicant.CNICNumber, x.RelativeName, x.Department, x.Designation, x.PayScale, x.ApplicantId })
            .AsAsyncEnumerable();

        await foreach (var row in query)
            await s.AddRowAsync(new List<DataCell> { 
                new(row.CNICNumber ?? ""), new(row.RelativeName ?? ""), new(row.Department ?? ""), 
                new(row.Designation ?? ""), new(row.PayScale ?? ""), new(row.ApplicantId.ToString()) 
            });
    }

    private async Task WriteDocumentsSheet(Spreadsheet s, StyleId h, Guid? campaignId)
    {
        string[] headers = { "CNIC", "DocType", "FileUrl", "ApplicantId" };
        await s.AddRowAsync(headers.Select(x => new StyledCell(x, h)).ToList());

        var query = context.Set<ApplicantDocument>().AsNoTracking()
            .Where(x => context.JobApplications.Any(ja => ja.ApplicantId == x.ApplicantId && ja.JobOpening.CampaignId == campaignId))
            .Select(x => new { x.Applicant.CNICNumber, x.DocumentType, x.FileUrl, x.ApplicantId })
            .AsAsyncEnumerable();

        await foreach (var row in query)
            await s.AddRowAsync(new List<DataCell> { new(row.CNICNumber ?? ""), new(row.DocumentType ?? ""), new(row.FileUrl ?? ""), new(row.ApplicantId.ToString()) });
    }

    private async Task WriteAchievementSheet(Spreadsheet s, StyleId h, Guid? campaignId)
    {
        string[] headers = { "CNIC", "Achievement", "Organization", "Year", "ApplicantId" };
        await s.AddRowAsync(headers.Select(x => new StyledCell(x, h)).ToList());

        var query = context.Set<ApplicantAchievement>().AsNoTracking()
            .Where(x => context.JobApplications.Any(ja => ja.ApplicantId == x.ApplicantId && ja.JobOpening.CampaignId == campaignId))
            .Select(x => new { x.Applicant.CNICNumber, x.Title, x.Description, x.DateReceived, x.ApplicantId })
            .AsAsyncEnumerable();

        await foreach (var row in query)
            await s.AddRowAsync(new List<DataCell> { 
                new(row.CNICNumber ?? ""), new(row.Title ?? ""), new(row.Description ?? ""), 
                new(row.DateReceived.ToString() ?? ""), new(row.ApplicantId.ToString()) 
            });
    }

    #endregion

    #region File Copy Logic (Batching + Semaphore)

    private async Task ProcessFileCopiesAsync(CampaignExportTask task, string tempRoot, string baseRoot)
    {
        int skip = 0;
        int batchSize = 30; // Pulling 100 applicant file mappings into memory at a time
        bool hasData = true;

        // SemaphoreSlim restricts the max degree of concurrency for actual Disk I/O Operations.
        int maxConcurrentFiles = config.GetValue<int>("ExportSettings:MaxConcurrentFileCopies", 10);
        using var semaphore = new SemaphoreSlim(maxConcurrentFiles);

        while (hasData)
        {
            // PROJECTION: Only grab the exact file path strings we need to copy. (Significantly reduces DB load)
            var batchApps = await context.JobApplications.AsNoTracking()
                .Where(a => a.JobOpening.CampaignId == task.CampaignId)
                .OrderBy(a => a.Id)
                .Skip(skip)
                .Take(batchSize)
                .Select(a => new
                {
                    JobTitle = a.JobOpening.Title,
                    FullName = a.Applicant.FullName,
                    CNIC = a.Applicant.CNICNumber,
                    PassportUrl = a.Applicant.PassportImageUrl,
                    CvUrl = a.Applicant.CvUrl,
                    Docs = a.Applicant.Documents.Select(d => new { d.DocumentType, d.FileUrl }).ToList()
                })
                .ToListAsync();

            if (batchApps.Count == 0) break;

            // Prepare asynchronous tasks for this batch
            var copyTasks = batchApps.Select(async app =>
            {
                await semaphore.WaitAsync(); // Wait for an available execution slot
                try
                {
                    string appDir = Path.Combine(tempRoot, Sanitize(app.JobTitle ?? "Job"), Sanitize($"{app.FullName}_{app.CNIC}"));

                    if (!string.IsNullOrEmpty(app.PassportUrl))
                        await SafeCopy(baseRoot, app.PassportUrl, Path.Combine(appDir, "Photo"), "Profile");

                    if (!string.IsNullOrEmpty(app.CvUrl))
                        await SafeCopy(baseRoot, app.CvUrl, Path.Combine(appDir, "CV"), "Main_CV");

                    foreach (var doc in app.Docs)
                        await SafeCopy(baseRoot, doc.FileUrl, Path.Combine(appDir, "Documents", Sanitize(doc.DocumentType ?? "Other")), "Doc");
                }
                finally
                {
                    semaphore.Release(); // Free the slot for the next file
                }
            });

            await Task.WhenAll(copyTasks); // Execute batch safely with semaphore cap

            skip += batchApps.Count;
            await UpdateTaskStatus(task, $"Files copied: {skip} applicants...");
            await Log("FILES", $"Processed batch skip {skip}.");
        }
    }

    private async Task SafeCopy(string root, string? relPath, string targetDir, string prefix)
    {
        if (string.IsNullOrEmpty(relPath)) return;
        try
        {
            string src = Path.GetFullPath(Path.Combine(root, relPath.Replace("\\", "/").TrimStart('/')));
            if (!File.Exists(src)) return;

            Directory.CreateDirectory(targetDir);
            string dest = Path.Combine(targetDir, $"{prefix}_{Guid.NewGuid():N}{Path.GetExtension(src)}");

            // Increased buffer to 80KB for optimum async disk I/O throughput
            await using var s = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true);
            await using var d = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);
            await s.CopyToAsync(d);
        }
        catch (Exception ex) 
        { 
            await Log("FILE ERROR", $"Failed to copy {relPath}: {ex.Message}"); 
        }
    }

    #endregion

    #region Helpers

    private async Task UpdateTaskStatus(CampaignExportTask task, string status)
    {
        task.Status = status;
        await context.SaveChangesAsync();
    }

    private async Task Log(string category, string message)
    {
        string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{category.PadRight(12)}] {message}{Environment.NewLine}";
        await File.AppendAllTextAsync(_logFilePath, logLine);
        logger.LogInformation("{Category}: {Message}", category, message);
    }

    private string Sanitize(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "Unknown";
        var invalid = Path.GetInvalidFileNameChars();
        return new string(name.Select(c => invalid.Contains(c) ? '_' : c).ToArray()).Replace(" ", "_").Trim();
    }

    #endregion
}