using System.ComponentModel.DataAnnotations;

namespace Career635.Areas.Admin.Models;

public class EditJobViewModel : CreateJobViewModel // Re-use fields from Create
{
    public Guid Id { get; set; }
}

public class EditCampaignViewModel
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
}