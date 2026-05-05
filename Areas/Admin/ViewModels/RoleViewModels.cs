using System.ComponentModel.DataAnnotations;

namespace Career635.Areas.Admin.Models;

public class CreateRoleViewModel
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    // List of Permission IDs selected from the checkbox matrix
    public List<Guid> SelectedPermissionIds { get; set; } = new();
}

public record PermissionGroupDto(string Module, IEnumerable<PermissionItemDto> Permissions);
public record PermissionItemDto(Guid Id, string DisplayName, string Name);




public record RoleSummaryViewModel(
    Guid Id,
    string Name,
    string? Description,
    int PermissionCount,
    int UserCount
);

public record RoleIndexViewModel(
    IEnumerable<RoleSummaryViewModel> Items,
    int PageNumber,
    int TotalPages,
    int TotalCount
) {
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public class EditRoleViewModel
{
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    // List of Permission IDs currently checked in the UI
    public List<Guid> SelectedPermissionIds { get; set; } = new();
}