using System.ComponentModel.DataAnnotations;

namespace CompanyService.DTOs;
public class CompanyCreateDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string County { get; set; }

    [Required]
    public string Description { get; set; }
}

