using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Info.Common.Repository;

namespace Info.CompanyService.Models;

[Table(name: "Companies")]
public class Company : EntityBase
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string County { get; set; }

    [Required]
    public string Description { get; set; }
}