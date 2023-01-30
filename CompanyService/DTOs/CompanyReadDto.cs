using System.ComponentModel.DataAnnotations;

namespace CompanyService.DTOs;
public class CompanyReadDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string County { get; set; }

    public string Description { get; set; }
}

