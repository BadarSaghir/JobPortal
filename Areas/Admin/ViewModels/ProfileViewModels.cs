using System.ComponentModel.DataAnnotations;

namespace Career635.Areas.Admin.Models;

public record UserProfileViewModel(
    string FullName,
    string Email,
    string UserName,
    string? Designation,
    string? Department,
    string? PayScale,
    DateTime CreatedAt,
    IEnumerable<string> Roles,
    IEnumerable<string> Permissions,
        bool IsTwoFactorEnabled, // NEW
    bool HasAuthenticator    // NEW

);

public class UpdateProfileViewModel
{
    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}