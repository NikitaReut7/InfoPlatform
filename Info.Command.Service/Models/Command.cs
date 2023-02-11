using Info.Common.Repository;
using System.ComponentModel.DataAnnotations;

namespace Info.CommandService.Models;
public class Command : EntityBase
{

    [Required]
    public string HowTo { get; set; }

    [Required]
    public string CommandLine { get; set; }

    [Required]
    public int PlatformId { get;  set; }

    public Platform Platform { get; set; }
}

