namespace FlashCard.Model.DTO.TranslationDto;

public class TranslationResponse
{
	public Guid TranslationId { get; set; }
	public string SourceWord { get; set; }
	public string TargetWord { get; set; }
	public string SourceLanguageName { get; set; }
	public string TargetLanguageName { get; set; }
	public string SourceLevelName { get; set; }
	public string? Image { get; set; }
}