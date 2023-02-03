using System.ComponentModel.DataAnnotations;

namespace Info.CompanyService.DTOs;
public class CompanyUpdateDto
{
    [Required]
    public string Name { get; set; }

    public string County { get; set; }

    public string Description { get; set; }
}

