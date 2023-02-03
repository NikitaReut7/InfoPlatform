using System.ComponentModel.DataAnnotations;

namespace Info.PlatformService.Models;

public class Company
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int ExternalId { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Platform> Platforms { get; set; } = new List<Platform>();


}

