public interface IFileStorageService {
    Task<string> SaveApplicantFileAsync(IFormFile file, string jobId, string cnic, string applicantId, string category,string campId="Default");
    
}

public class FileStorageService(IConfiguration config, IWebHostEnvironment env) : IFileStorageService
{
    public async Task<string> SaveApplicantFileAsync(IFormFile file, string jobId, string cnic, string applicantId, string category,string campId="Default")
    {
        if (file == null || file.Length == 0) return string.Empty;

        // 1. Validate Size (50KB)
        long maxSize = config.GetValue<long>("StorageSettings:MaxFileSizeBytes");
        if (file.Length > maxSize) 
            throw new Exception($"File {file.FileName} exceeds the {maxSize/1024}KB limit.");

        // 2. Build Cross-Platform Path
        // Format: Root/JobId/CNIC/ApplicantId/Category/filename
        string root = config.GetValue<string>("StorageSettings:DocumentRoot") ?? "uploads";
        
        // Ensure path is absolute if starts with wwwroot, otherwise relative to App Base
        string baseDir = root.StartsWith("wwwroot") 
            ? Path.Combine(env.ContentRootPath, root) 
            : root;

        string targetFolder = Path.Combine(baseDir,campId ,jobId, cnic, applicantId, category);

        if (!Directory.Exists(targetFolder)) 
            Directory.CreateDirectory(targetFolder);

        // 3. Sanitize and Save
        string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        string filePath = Path.Combine(targetFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return relative path for Database storage
        return Path.Combine(campId,jobId, cnic, applicantId, category, fileName);
    }
}