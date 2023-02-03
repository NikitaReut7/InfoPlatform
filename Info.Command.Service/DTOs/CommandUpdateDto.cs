using System.ComponentModel.DataAnnotations;

namespace Info.CommandService.DTOs;
public class CommandUpdateDto
{
   
    [Required]
    public string HowTo { get; set; }

    [Required]
    public string CommandLine { get; set; }

}

