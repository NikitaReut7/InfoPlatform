using System.ComponentModel.DataAnnotations;

namespace Info.Common.Repository;

public abstract class EntityBase
{
    [Key]
    [Required]
    public int Id { get; set; }
}
