namespace Career635.Domain.Constants;

public static class AppPermissions
{
    public const string SystemAll = "system.all";
    
    public const string JobsView = "jobs.view";
    public const string JobsManage = "jobs.manage";
    
    public const string ApplicantsView = "applicants.view";
    public const string ApplicantsManage = "applicants.manage";
    public const string ApplicantsExport = "applicants.export";
    
    public const string CampaignsManage = "campaigns.manage";
    public const string RolesManage = "roles.manage";
    public const string StaffManage = "staff.manage";

    public static List<(string Name, string Module, string Display)> AllPermissions => new()
    {
        (SystemAll, "System", "Full System Access"),
        (JobsView, "Jobs", "View Vacancy Registry"),
        (JobsManage, "Jobs", "Create and Edit Vacancies"),
        (ApplicantsView, "Applicants", "View Candidate Dossiers"),
        (ApplicantsManage, "Applicants", "Update Application Status"),
        (ApplicantsExport, "Applicants", "Export Campaign Data"),
        (CampaignsManage, "Campaigns", "Manage Recruitment Batches"),
        (RolesManage, "Security", "Manage Roles and Privileges"),
        (StaffManage, "Security", "Manage Internal Personnel")
    };
}