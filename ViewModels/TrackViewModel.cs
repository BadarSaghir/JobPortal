namespace Career635.Features.Jobs.Models;

public record TrackViewModel(
    string? SearchValue,
    bool Searched,
    TrackResultViewModel? Result
);

public record TrackResultViewModel(
    List<ApplicationStatusViewModel> Applications,
    bool IsCnicSearch,
    int CurrentPage,
    int TotalPages,
    int TotalCount
);

public record ApplicationStatusViewModel(
    Guid ApplicationId,
    string JobTitle,
    DateTimeOffset AppliedAt,
    string Status,
    string? Remarks,
    string TrackingCode,
    ApplicantDossierViewModel Dossier
);

// Full 8-Page Data for Printing
public record ApplicantDossierViewModel(
    string FullName, string CNIC, string? PhotoUrl,
    PersonalInfoVM Personal,
    FamilyVM Family,
    FinancialVM Financial,
    MilitaryVM Military,
    List<EducationVM> Educations,
    List<ExperienceVM> Experiences,
    List<SiblingVM> Siblings,
    List<RelativeVM> Relatives,
    List<CertVM> Certifications,
    List<SkillVM> Skills,
    List<AchVM> Achievements
);

public record PersonalInfoVM(string FatherName, string? FatherCNIC, DateTime DOB, string Gender, string MaritalStatus, string Religion, string? Caste, string? Sect, string ContactNo, string? Email, string? PEC, string PresentAddress, string PermanentAddress, string? Accommodation);
public record FamilyVM(int BTotal, int BMar, int BUnmar, int STotal, int SMar, int SUnmar, int CTotal, int CMar, int CUnmar);
public record FinancialVM(decimal? Current, decimal? Expected, string? Benefits, string? Income, string? Facilities);
public record MilitaryVM(string? No, string? Unit, string? Character, string? Scale);
public record EducationVM(string Qualification, string Major, string Institute, string Result, DateTime From, DateTime To);
public record ExperienceVM(string Org, string Designation, string? Responsibilities, DateTime From, DateTime? To);
public record SiblingVM(string Name, string? CNIC, string? Gender, string? Occupation, string? MaritalStatus);
public record RelativeVM(string Name, string Designation, string Dept, string Scale);
public record CertVM(string Name, string Body);
public record SkillVM(string Name, string Level);
public record AchVM(string Title, string? Desc, DateTime Date);