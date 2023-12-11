using System.ComponentModel.DataAnnotations;

namespace FlashCard.Model.Domain;

public class Language
{
    [Key]
    public Guid LanguageId { get; set; }
    public string LanguageName { get; set; }
}

