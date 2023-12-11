using FlashCard.Model.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Word
{
	[Key]
	public Guid WordId { get; set; }
	public string WordText { get; set; }
	public Guid LanguageId { get; set; }
	public Guid LevelId { get; set; }
	public string? ImageUrl { get; set; }
	[ForeignKey("LanguageId")]
	public Language Language { get; set; }
	[ForeignKey("LevelId")]
	public Level Level { get; set; }
}