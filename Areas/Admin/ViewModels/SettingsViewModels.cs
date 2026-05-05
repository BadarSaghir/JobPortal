using System.ComponentModel.DataAnnotations;

namespace Career635.Areas.Admin.Models;

public class ChangePasswordViewModel
{
    [Required, DataType(DataType.Password), Display(Name = "Current Password")]
    public string OldPassword { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 8), DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [DataType(DataType.Password), Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class SystemPreferencesViewModel
{
    public bool EmailNotifications { get; set; } = true;
    public bool CompactMode { get; set; } = false;
    public string DefaultDashboard { get; set; } = "Overview";
}