namespace FlashCard.Model.DTO.WordDto;

public class WordResponse
{
	public Guid WordId { get; set; }
	public string WordText { get; set; }
	public string Language { get; set; }
	public string Level { get; set; }
	public string? ImageUrl { get; set; }
}