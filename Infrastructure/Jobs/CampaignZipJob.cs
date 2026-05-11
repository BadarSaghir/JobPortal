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

        string storageRoot = config.GetValue<string>("StorageSettings:DocumentRoot") ?? "uploads";
        string tempRoot = Path.Combine(storageRoot, "temp_exports", taskId.ToString());

        try
        {
            task.Status = "Processing";
            task.ProcessedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            // Clean up any existing temp directory
            if (Directory.Exists(tempRoot)) 
            {
                try { Directory.Delete(tempRoot, true); }
                catch { /* Ignore cleanup errors */ }
            }
            Directory.CreateDirectory(tempRoot);

            string baseStoragePath = storageRoot.StartsWith("wwwroot") 
                ? Path.Combine(env.ContentRootPath, storageRoot) 
                : storageRoot;

            // Get total counts for progress tracking
            var totalJobs = await context.JobOpenings
                .Where(j => j.CampaignId == task.CampaignId)
                .CountAsync();

            var totalApplicants = await context.JobApplications
                .Include(a => a.JobOpening)
                .Where(a => a.JobOpening.CampaignId == task.CampaignId)
                .CountAsync();

            // task.TotalRecords = totalApplicants;
            await context.SaveChangesAsync();

            // Process everything
            string excelPath = Path.Combine(tempRoot, $"Campaign_Registry_{task.Campaign?.CampaignCode ?? "Default"}.xlsx");
            
            await ProcessExportAsync(task, tempRoot, excelPath, baseStoragePath, totalJobs, totalApplicants);

            // Create final ZIP file
            string exportDir = Path.Combine(baseStoragePath, "exports");
            if (!Directory.Exists(exportDir)) Directory.CreateDirectory(exportDir);

            string zipFileName = $"{task.Campaign?.CampaignCode ?? "Default"}_Full_Candidates_{DateTime.Now:yyyyMMddHHmmss}.zip";
            string finalZipPath = Path.Combine(exportDir, zipFileName);

            if (File.Exists(finalZipPath)) File.Delete(finalZipPath);
            
            // Create ZIP with optimized settings
            ZipFile.CreateFromDirectory(tempRoot, finalZipPath, CompressionLevel.Optimal, false);

            task.Status = "Completed";
            task.DownloadUrl = $"/uploads/exports/{zipFileName}";
            task.ProcessedAt = DateTime.UtcNow;
            // task.CompletedAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Export Error on Task {TaskId}", taskId);
            task.Status = "Failed";
            task.ErrorMessage = ex.Message;
            task.ProcessedAt = DateTime.UtcNow;
        }
        finally
        {
            // Clean up temp directory
            if (Directory.Exists(tempRoot)) 
            {
                try 
                { 
                    Directory.Delete(tempRoot, true); 
                    logger.LogInformation("Cleaned up temp directory for task {TaskId}", taskId);
                }
                catch (Exception ex) 
                { 
                    logger.LogWarning(ex, "Failed to cleanup temp directory for task {TaskId}", taskId);
                }
            }
        }

        await context.SaveChangesAsync();
    }

    private async Task ProcessExportAsync(CampaignExportTask task, string tempRoot, string excelPath, 
        string baseStoragePath, int totalJobs, int totalApplicants)
    {
        int batchSize = config.GetValue<int>("ExportSettings:BatchSize", 25);
        int skip = 0;
        int processedApplicants = 0;
        int processedJobs = 0;

        // Create Excel workbook with streaming approach
        using (var workbook = new XLWorkbook())
        {
            // Create all worksheets
            var wsMaster = workbook.Worksheets.Add("Master Registry");
            var wsEdu = workbook.Worksheets.Add("Education");
            var wsExp = workbook.Worksheets.Add("Experience");
            var wsSib = workbook.Worksheets.Add("Siblings");
            var wsExtra = workbook.Worksheets.Add("Skills_Certs");
            var wsRelatives = workbook.Worksheets.Add("Relatives");
            var wsDoc = workbook.Worksheets.Add("Documents");
            var wsAchievements = workbook.Worksheets.Add("Achievements");

            // Add headers to all worksheets
            AddMasterHeaders(wsMaster);
            AddSimpleHeaders(wsEdu, new[] { "CNIC", "Level", "Qualification", "Institution", "Result","From", "To", "ApplicantId" });
            AddSimpleHeaders(wsExp, new[] { "CNIC", "Organization", "Designation", "From", "To", "KeyResponsibilities", "ApplicantId" });
            AddSimpleHeaders(wsSib, new[] { "CNIC", "Name", "Gender", "Occupation", "SiblingCNIC", "DateOfBirth", "Designation", "Organization", "ApplicantId" });
            AddSimpleHeaders(wsExtra, new[] { "CNIC", "Type", "Name", "Detail", "ApplicantId" });
            AddSimpleHeaders(wsRelatives, new[] { "CNIC", "RelativeName", "Department", "Designation", "PayScale", "ApplicantId" });
            AddSimpleHeaders(wsDoc, new[] { "CNIC", "DocType", "FileUrl", "ApplicantId" });
            AddSimpleHeaders(wsAchievements, new[] { "CNIC", "Achievement", "Organization", "Year", "ApplicantId" });

            int masterRow = 2, eduRow = 2, expRow = 2, sibRow = 2, extraRow = 2, relRow = 2, docRow = 2, achRow = 2;

            // Process jobs in batches
            while (skip < totalJobs)
            {
                var jobsBatch = await context.JobOpenings
                    .AsNoTracking()
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.PersonalInfo)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.FamilySummary)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.MilitaryDetail)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.FinancialDetail)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.Educations)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.Experiences)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.Siblings)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.Skills)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.Certifications)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.Achievements)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.InternalRelatives)
                    .Include(j => j.JobApplications)
                        .ThenInclude(a => a.Applicant)
                            .ThenInclude(x => x.Documents)
                    .Where(j => j.CampaignId == task.CampaignId)
                    .OrderBy(j => j.Id)
                    .Skip(skip)
                    .Take(batchSize)
                    .ToListAsync();

                if (!jobsBatch.Any()) break;

                foreach (var job in jobsBatch)
                {
                    processedJobs++;
                    
                    // Update progress
                    if (processedJobs % 5 == 0)
                    {
                        task.Status = $"Processing job {processedJobs}/{totalJobs}...";
                        // task.ProcessedAt = processedApplicants;
                        await context.SaveChangesAsync();
                    }

                    // Copy files for this job's applicants
                    await CopyFilesForJobAsync(job, tempRoot, baseStoragePath);
                    
                    // Write to Excel worksheets
                    foreach (var app in job.JobApplications)
                    {
                        processedApplicants++;
                        
                        WriteMasterRecord(wsMaster, masterRow++, app);
                        WriteEducationRecords(wsEdu, ref eduRow, app);
                        WriteExperienceRecords(wsExp, ref expRow, app);
                        WriteSiblingRecords(wsSib, ref sibRow, app);
                        WriteSkillCertRecords(wsExtra, ref extraRow, app);
                        WriteRelativeRecords(wsRelatives, ref relRow, app);
                        WriteDocumentRecords(wsDoc, ref docRow, app);
                        WriteAchievementRecords(wsAchievements, ref achRow, app);
                        
                        // Periodic save to release memory
                        if (processedApplicants % 500 == 0)
                        {
                            workbook.SaveAs(excelPath + ".tmp");
                            task.Status = $"Processed {processedApplicants}/{totalApplicants} applicants, saving...";
                            await context.SaveChangesAsync();
                            
                            // Force garbage collection
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                    }
                }

                skip += batchSize;
                
                // Clear tracking and force GC every few batches
                if (skip % (batchSize * 10) == 0)
                {
                    context.ChangeTracker.Clear();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            // Apply final formatting
            foreach (var ws in workbook.Worksheets)
            {
                ws.Columns().AdjustToContents();
                ws.SheetView.FreezeRows(1);
                
                // Apply header styling
                var headerRange = ws.Range(1, 1, 1, ws.ColumnsUsed().Count());
                headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#064E3B");
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Font.Bold = true;
            }

            // Save final Excel file
            workbook.SaveAs(excelPath);
            
            // Clean up temp file if exists
            if (File.Exists(excelPath + ".tmp"))
                File.Delete(excelPath + ".tmp");
        }
    }

    private void AddMasterHeaders(IXLWorksheet ws)
    {
        var headers = new[] 
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

        for (int i = 0; i < headers.Length; i++)
            ws.Cell(1, i + 1).Value = headers[i];
    }

    private void AddSimpleHeaders(IXLWorksheet ws, string[] headers)
    {
        for (int i = 0; i < headers.Length; i++)
            ws.Cell(1, i + 1).Value = headers[i];
    }

    private void WriteMasterRecord(IXLWorksheet ws, int row, JobApplication app)
    {
        var a = app.Applicant;
        var p = a.PersonalInfo;
        var m = a.MilitaryDetail;
        var f = a.FinancialDetail;
        var fs = a.FamilySummary;

        ws.Cell(row, 1).Value = a.TrackingCode ?? "N/A";
        ws.Cell(row, 2).Value = app.JobOpening?.Title ?? "N/A";
        ws.Cell(row, 3).Value = app.Status ?? "N/A";
        ws.Cell(row, 4).Value = app.AppliedAt.DateTime;
        ws.Cell(row, 5).Value = a.FullName ?? "N/A";
        ws.Cell(row, 6).Value = a.CNICNumber ?? "N/A";
        ws.Cell(row, 7).Value = p?.FatherName ?? "N/A";
        ws.Cell(row, 8).Value = p?.FatherCNIC ?? "N/A";
        ws.Cell(row, 9).Value = p?.DateOfBirth;
        ws.Cell(row, 10).Value = p?.Gender ?? "N/A";
        ws.Cell(row, 11).Value = p?.MaritalStatus ?? "N/A";
        ws.Cell(row, 12).Value = p?.Religion ?? "N/A";
        ws.Cell(row, 13).Value = p?.Caste ?? "N/A";
        ws.Cell(row, 14).Value = p?.Sect ?? "N/A";
        ws.Cell(row, 15).Value = p?.ContactNo ?? "N/A";
        ws.Cell(row, 16).Value = p?.Email ?? "N/A";
        ws.Cell(row, 17).Value = p?.PECNumber ?? "N/A";
        ws.Cell(row, 18).Value = p?.PresentAddress ?? "N/A";
        ws.Cell(row, 19).Value = p?.PermanentAddress ?? "N/A";
        ws.Cell(row, 20).Value = m?.ArmyNumber ?? "N/A";
        ws.Cell(row, 21).Value = m?.ArmyUnit ?? "N/A";
        ws.Cell(row, 22).Value = m?.ArmyCharacter ?? "N/A";
        ws.Cell(row, 23).Value = m?.ArmyPayScale ?? "N/A";
        ws.Cell(row, 24).Value = f?.CurrentSalary.ToString() ?? "N/A";
        ws.Cell(row, 25).Value = f?.OtherBenefits ?? "N/A";
        ws.Cell(row, 26).Value = f?.OtherFacilities ?? "N/A";
        ws.Cell(row, 27).Value = f?.ExpectedSalary.ToString() ?? "N/A";
        ws.Cell(row, 28).Value = f?.FamilyIncomeDetail ?? "N/A";
        ws.Cell(row, 29).Value = fs?.BrothersTotal ?? 0;
        ws.Cell(row, 30).Value = fs?.SistersTotal ?? 0;
        ws.Cell(row, 31).Value = fs?.ChildrenTotal ?? 0;
        ws.Cell(row, 32).Value = p?.CandidateType ?? "N/A";
        ws.Cell(row, 33).Value = p?.Accommodation ?? "N/A";
        ws.Cell(row, 34).Value = fs?.SistersMarried ?? 0;
        ws.Cell(row, 35).Value = fs?.BrothersMarried ?? 0;
        ws.Cell(row, 36).Value = fs?.ChildrenMarried ?? 0;
        ws.Cell(row, 37).Value = fs?.SistersUnmarried ?? 0;
        ws.Cell(row, 38).Value = fs?.BrothersUnmarried ?? 0;
        ws.Cell(row, 39).Value = fs?.ChildrenUnmarried ?? 0;
        ws.Cell(row, 40).Value = app.JobOpeningId.ToString();
        ws.Cell(row, 41).Value = app.JobOpening?.Title ?? "N/A";
        ws.Cell(row, 42).Value = app.Applicant?.CvUrl ?? "N/A";
        ws.Cell(row, 43).Value = app.Applicant?.PassportImageUrl ?? "N/A";
        ws.Cell(row, 44).Value = app.ApplicantId.ToString();
    }

    private void WriteEducationRecords(IXLWorksheet ws, ref int row, JobApplication app)
    {
        if (app.Applicant.Educations == null || !app.Applicant.Educations.Any()) return;
        
        foreach (var edu in app.Applicant.Educations)
        {
            ws.Cell(row, 1).Value = app.Applicant.CNICNumber ?? "N/A";
            ws.Cell(row, 2).Value = edu.DegreeLevel?.Name ?? "N/A";
            ws.Cell(row, 3).Value = edu.Qualification ?? "N/A";
            ws.Cell(row, 4).Value = edu.BoardUniversity ?? "N/A";
            ws.Cell(row, 5).Value = edu.CgpaPercentage ?? "N/A";
            ws.Cell(row, 6).Value = edu.FromDate.ToString() ?? "N/A";
            ws.Cell(row, 7).Value = edu.ToDate.ToString() ?? "N/A";
            ws.Cell(row, 8).Value = app.ApplicantId.ToString();
            row++;
        }
    }

    private void WriteExperienceRecords(IXLWorksheet ws, ref int row, JobApplication app)
    {
        if (app.Applicant.Experiences == null || !app.Applicant.Experiences.Any()) return;
        
        foreach (var exp in app.Applicant.Experiences)
        {
            ws.Cell(row, 1).Value = app.Applicant.CNICNumber ?? "N/A";
            ws.Cell(row, 2).Value = exp.OrganizationName ?? "N/A";
            ws.Cell(row, 3).Value = exp.Designation ?? "N/A";
            ws.Cell(row, 4).Value = exp.FromDate;
            ws.Cell(row, 5).Value = exp.ToDate;
            ws.Cell(row, 6).Value = exp.KeyResponsibilities ?? "N/A";
            ws.Cell(row, 7).Value = app.ApplicantId.ToString();
            row++;
        }
    }

    private void WriteSiblingRecords(IXLWorksheet ws, ref int row, JobApplication app)
    {
        if (app.Applicant.Siblings == null || !app.Applicant.Siblings.Any()) return;
        
        foreach (var sibling in app.Applicant.Siblings)
        {
            ws.Cell(row, 1).Value = app.Applicant.CNICNumber ?? "N/A";
            ws.Cell(row, 2).Value = sibling.Name ?? "N/A";
            ws.Cell(row, 3).Value = sibling.Gender ?? "N/A";
            ws.Cell(row, 4).Value = sibling.Occupation ?? "N/A";
            ws.Cell(row, 5).Value = sibling.CNIC ?? "N/A";
            ws.Cell(row, 6).Value = sibling.DateOfBirth;
            ws.Cell(row, 7).Value = sibling.Designation ?? "N/A";
            ws.Cell(row, 8).Value = sibling.Organization ?? "N/A";
            ws.Cell(row, 9).Value = app.ApplicantId.ToString();
            row++;
        }
    }

    private void WriteSkillCertRecords(IXLWorksheet ws, ref int row, JobApplication app)
    {
        // Write Skills
        if (app.Applicant.Skills != null)
        {
            foreach (var skill in app.Applicant.Skills)
            {
                ws.Cell(row, 1).Value = app.Applicant.CNICNumber ?? "N/A";
                ws.Cell(row, 2).Value = "Skill";
                ws.Cell(row, 3).Value = skill.SkillName ?? "N/A";
                ws.Cell(row, 4).Value = skill.Proficiency ?? "N/A";
                ws.Cell(row, 5).Value = app.ApplicantId.ToString();
                row++;
            }
        }
        
        // Write Certifications
        if (app.Applicant.Certifications != null)
        {
            foreach (var cert in app.Applicant.Certifications)
            {
                ws.Cell(row, 1).Value = app.Applicant.CNICNumber ?? "N/A";
                ws.Cell(row, 2).Value = "Certification";
                ws.Cell(row, 3).Value = cert.CertificateName ?? "N/A";
                ws.Cell(row, 4).Value = cert.IssuingBody ?? "N/A";
                ws.Cell(row, 5).Value = app.ApplicantId.ToString();
                row++;
            }
        }
    }

    private void WriteRelativeRecords(IXLWorksheet ws, ref int row, JobApplication app)
    {
        if (app.Applicant.InternalRelatives == null || !app.Applicant.InternalRelatives.Any()) return;
        
        foreach (var relative in app.Applicant.InternalRelatives)
        {
            ws.Cell(row, 1).Value = app.Applicant.CNICNumber ?? "N/A";
            ws.Cell(row, 2).Value = relative.RelativeName ?? "N/A";
            ws.Cell(row, 3).Value = relative.Department ?? "N/A";
            ws.Cell(row, 4).Value = relative.Designation ?? "N/A";
            ws.Cell(row, 5).Value = relative.PayScale ?? "N/A";
            ws.Cell(row, 6).Value = app.ApplicantId.ToString();
            row++;
        }
    }

    private void WriteDocumentRecords(IXLWorksheet ws, ref int row, JobApplication app)
    {
        if (app.Applicant.Documents == null || !app.Applicant.Documents.Any()) return;
        
        foreach (var doc in app.Applicant.Documents)
        {
            ws.Cell(row, 1).Value = app.Applicant.CNICNumber ?? "N/A";
            ws.Cell(row, 2).Value = doc.DocumentType ?? "N/A";
            ws.Cell(row, 3).Value = doc.FileUrl ?? "N/A";
            ws.Cell(row, 4).Value = app.ApplicantId.ToString();
            row++;
        }
    }

    private void WriteAchievementRecords(IXLWorksheet ws, ref int row, JobApplication app)
    {
        if (app.Applicant.Achievements == null || !app.Applicant.Achievements.Any()) return;
        
        foreach (var achievement in app.Applicant.Achievements)
        {
            ws.Cell(row, 1).Value = app.Applicant.CNICNumber ?? "N/A";
            ws.Cell(row, 2).Value = achievement.Title ?? "N/A";
            ws.Cell(row, 3).Value = achievement.Description ?? "N/A";
            ws.Cell(row, 4).Value = achievement.DateReceived.ToString() ??"N/A";
            ws.Cell(row, 5).Value = app.ApplicantId.ToString();
            row++;
        }
    }

    private async Task CopyFilesForJobAsync(JobOpening job, string tempRoot, string baseStoragePath)
    {
        var semaphore = new SemaphoreSlim(config.GetValue<int>("ExportSettings:MaxConcurrentFileCopies", 5));
        var copyTasks = new List<Task>();

        foreach (var app in job.JobApplications)
        {
            var task = Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await CopyApplicantFilesAsync(app, job, tempRoot, baseStoragePath);
                }
                finally
                {
                    semaphore.Release();
                }
            });
            copyTasks.Add(task);
        }

        await Task.WhenAll(copyTasks);
        semaphore.Dispose();
    }

    private async Task CopyApplicantFilesAsync(JobApplication app, JobOpening job, string tempRoot, string baseStoragePath)
    {
        // Create folder structure
        string jobFolderName = Sanitize($"{job.Title}_{job.Id.ToString()[..Math.Min(8, job.Id.ToString().Length)]}");
        string jobPath = Path.Combine(tempRoot, jobFolderName);
        
        string applicantFolderName = Sanitize($"{app.Applicant.FullName}_{app.Applicant.CNICNumber}");
        string applicantPath = Path.Combine(jobPath, applicantFolderName);
        
        bool hasFiles = false;

        // Copy Profile Photo
        if (!string.IsNullOrEmpty(app.Applicant.PassportImageUrl))
        {
            string profilePath = Path.Combine(applicantPath, "Profile_Photo");
            Directory.CreateDirectory(profilePath);
            await SafeCopyFileAsync(baseStoragePath, app.Applicant.PassportImageUrl, profilePath, "Passport_Photo");
            hasFiles = true;
        }

        // Copy CV
        if (!string.IsNullOrEmpty(app.Applicant.CvUrl))
        {
            string cvPath = Path.Combine(applicantPath, "CV_Portfolio");
            Directory.CreateDirectory(cvPath);
            await SafeCopyFileAsync(baseStoragePath, app.Applicant.CvUrl, cvPath, "Main_CV");
            hasFiles = true;
        }

        // Copy Documents
        if (app.Applicant.Documents != null && app.Applicant.Documents.Any())
        {
            string docsPath = Path.Combine(applicantPath, "Supporting_Documents");
            Directory.CreateDirectory(docsPath);
            
            foreach (var doc in app.Applicant.Documents)
            {
                string typeFolder = Path.Combine(docsPath, Sanitize(doc.DocumentType ?? "Uncategorized"));
                Directory.CreateDirectory(typeFolder);
                string filePrefix = $"Doc_{Guid.NewGuid():N}"[..Math.Min(20, Guid.NewGuid().ToString().Length)];
                await SafeCopyFileAsync(baseStoragePath, doc.FileUrl, typeFolder, filePrefix);
                hasFiles = true;
            }
        }

        // Remove empty folder if no files were copied
        if (!hasFiles && Directory.Exists(applicantPath))
        {
            try { Directory.Delete(applicantPath, true); }
            catch { /* Ignore cleanup errors */ }
        }
    }

    private async Task SafeCopyFileAsync(string baseRoot, string? relativePath, string targetDir, string fileNamePrefix)
    {
        if (string.IsNullOrEmpty(relativePath)) return;

        try
        {
            // Clean the path
            string cleanRelative = relativePath.Replace("\\", "/").TrimStart('/');
            string source = Path.GetFullPath(Path.Combine(baseRoot, cleanRelative));

            if (!File.Exists(source)) return;

            string ext = Path.GetExtension(source);
            if (string.IsNullOrEmpty(ext)) ext = ".dat";
            
            string dest = Path.Combine(targetDir, $"{fileNamePrefix}{ext}");
            
            // Handle duplicates
            int count = 1;
            while (File.Exists(dest)) 
            {
                dest = Path.Combine(targetDir, $"{fileNamePrefix}_{count++}{ext}");
            }
            
            // Copy file with buffered stream
            const int bufferSize = 81920; // 80KB buffer
            using (var sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous))
            using (var destStream = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, FileOptions.Asynchronous))
            {
                await sourceStream.CopyToAsync(destStream, bufferSize);
                await destStream.FlushAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to copy file {RelativePath} for prefix {FileNamePrefix}", relativePath, fileNamePrefix);
            // Don't throw - continue with other files
        }
    }

    private string Sanitize(string name)
    {
        if (string.IsNullOrEmpty(name)) return "Unknown";
        
        // Remove invalid characters
        var invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars) 
            name = name.Replace(c, '_');
        
        // Limit length
        if (name.Length > 100) 
            name = name[..100];
        
        return name.Replace(" ", "_").Trim('_');
    }
}