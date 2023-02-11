using System.ComponentModel.DataAnnotations;
using Info.Common.Repository;

namespace Info.PlatformService.Models;

public class Company : EntityBase
{
    [Required]
    public int ExternalId { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Platform> Platforms { get; set; } = new List<Platform>();


}

