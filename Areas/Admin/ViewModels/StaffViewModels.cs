using System.ComponentModel.DataAnnotations;

namespace Career635.Areas.Admin.Models;

public record StaffSummaryViewModel(
    Guid Id,
    string FullName,
    string Email,
    string? Department,
    string? Designation,
    IEnumerable<string> Roles,
    DateTime CreatedAt
);

public record StaffIndexViewModel(
    IEnumerable<StaffSummaryViewModel> Items,
    int PageNumber,
    int TotalPages,
    string? SearchTerm,
    Guid? DepartmentId
) {
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public class CreateStaffViewModel
{
    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    public Guid? DepartmentId { get; set; }
    public Guid? DesignationId { get; set; }
    public Guid? PayScaleId { get; set; }

    [Required]
    public string RoleName { get; set; } = "Recruiter";
}