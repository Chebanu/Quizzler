using System.ComponentModel.DataAnnotations;

public class Level
{
    [Key]
    public Guid LevelId { get; set; }
    public string LevelName { get; set; }
}