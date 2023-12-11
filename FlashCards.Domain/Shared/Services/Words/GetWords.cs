using FlashCard.Model;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Shared.Services.Translations;

public enum TypeOfQueryWord
{
	All,
	Level,
	Quantity,
	Language
}

public class GetWords
{
	private static async Task<List<Word>> GetFlashCards(FlashCardDbContext context,
													Func<IQueryable<Word>, IQueryable<Word>> filter = null)
	{
		var query = context.Words
					.Select(x => new Word
					{
						WordId = x.WordId,
						WordText = x.WordText,
						Language = x.Language,
						Level = x.Level,
						ImageUrl = x.ImageUrl
					});

		if (filter != null)
		{
			query = filter(query);
		}

		var randomCards = await query.ToListAsync();
		return randomCards;
	}


	public static async Task<List<Word>> GetFalshCardsByParameters(FlashCardDbContext context,
																	TypeOfQueryWord typeOfQueryWord,
																	string targetLang = null,
																	string level = null,
																	int quantity = 0)
	{
		var words = new List<Word>();

		switch (typeOfQueryWord)
		{
			case TypeOfQueryWord.All:
				words = await GetFlashCards(context);
				break;
			case TypeOfQueryWord.Level:
				if (!string.IsNullOrWhiteSpace(level) && !string.IsNullOrWhiteSpace(targetLang))
				{
					words = await GetFlashCards(context,
												query => query.Where(x => x.Language.LanguageName == targetLang)
													.Where(x => x.Level.LevelName == level));
				}
				break;
			case TypeOfQueryWord.Quantity:
				if (!string.IsNullOrWhiteSpace(targetLang) && quantity > 0)
				{
					words = await GetFlashCards(context,
												query => query.Where(x => x.Language.LanguageName == targetLang)
													.OrderBy(x => Guid.NewGuid())
													.Take(quantity));
				}
				break;
			case TypeOfQueryWord.Language:
				if (!string.IsNullOrEmpty(targetLang))
				{
					words = await GetFlashCards(context,
												query => query.Where(x => x.Language.LanguageName == targetLang));
				}
				break;
			default:
				throw new ArgumentException("Unsupported query type.", nameof(typeOfQueryWord));
		}

		return words;
	}
}