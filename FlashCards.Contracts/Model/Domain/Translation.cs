using System.ComponentModel.DataAnnotations.Schema;

public class Translation
{
	public Guid TranslationId { get; set; }
	public Guid SourceWordId { get; set; }
	public Guid TargetWordId { get; set; }

	[ForeignKey("SourceWordId")]
	public Word SourceWord { get; set; }

	[ForeignKey("TargetWordId")]
	public Word TargetWord { get; set; }
}
