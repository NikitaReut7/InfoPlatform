using System.ComponentModel.DataAnnotations;

namespace CompanyService.Models;

public class Company
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string County { get; set; }

    [Required]
    public string Description { get; set; }
}