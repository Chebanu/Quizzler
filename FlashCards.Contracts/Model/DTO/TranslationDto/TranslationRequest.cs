using System.ComponentModel.DataAnnotations;

namespace FlashCard.Model.DTO.TranslationDto;

public class TranslationRequest
{
	[Required]
	public string SourceWord { get; set; }
	[Required]
	public string SourceLanguage { get; set; }
	[Required]
	public string TargetWord { get; set; }
	[Required]
	public string TargetLanguage { get; set; }
}
