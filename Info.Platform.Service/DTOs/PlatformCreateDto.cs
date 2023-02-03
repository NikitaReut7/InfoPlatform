using System.ComponentModel.DataAnnotations;

namespace Info.PlatformService.DTOs;

public class PlatformCreateDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Cost { get; set; }
}

