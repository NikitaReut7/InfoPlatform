using System.ComponentModel.DataAnnotations;
using Info.Common.Repository;

namespace Info.PlatformService.Models;

public class Platform : EntityBase
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Cost { get; set; }

    [Required]
    public int CompanyId { get; set; }

    public Company Company { get; set; }
}