using System.ComponentModel.DataAnnotations;

namespace PlatformService.DTOs;

public class PlatformCreateDto
{
    [Required]
    public string Name { get; set; }

    public string Cost { get; set; }
}

