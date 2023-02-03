using System.ComponentModel.DataAnnotations;

namespace Info.PlatformService.Models;

public class Platform
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Cost {get;set;}

    [Required]
    public int CompanyId { get; set;}

    public Company Company { get; set; }
}