using FlashCard.Mediator.Translations;
using FlashCard.Model;
using FlashCard.Model.DTO.TranslationDto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Shared.Services.Translations;

public enum TypeOfQueryTranslation
{
	All,
	Level,
	Quantity
}

public class TranslationDistributor
{
	private static async Task<List<Translation>> GetFlashCards(FlashCardDbContext context,
													string sourceLang,
													string targetLang,
													Func<IQueryable<Translation>, IQueryable<Translation>> filter = null)
	{
		var query = context.Translations
						.Include(s => s.SourceWord.Language)
						.Include(s => s.TargetWord.Language)
						.Include(s => s.SourceWord.Level)
						.Include(s => s.TargetWord.Level)
						.Where(s => s.SourceWord.Language.LanguageName == sourceLang)
						.Where(r => r.TargetWord.Language.LanguageName == targetLang);

		if (filter != null)
		{
			query = filter(query);
		}

		var randomCards = await query
					.Select(x => new Translation
					{
						TranslationId = x.TranslationId,
						SourceWord = x.SourceWord,
						TargetWord = x.TargetWord,
					})
				.ToListAsync();

		return randomCards;
	}


	public static async Task<List<Translation>> GetFalshCardsByParameters(FlashCardDbContext context,
															TypeOfQueryTranslation typeOfQueryTranslation,
															string sourceLang,
															string targetLang,
															string level = null,
															int quantity = 0)
	{
		var translations = new List<Translation>();

		switch (typeOfQueryTranslation)
		{
			case TypeOfQueryTranslation.All:
				translations = await GetFlashCards(context, sourceLang, targetLang);
				break;
			case TypeOfQueryTranslation.Level:
				if (!string.IsNullOrWhiteSpace(level) &&
					!string.IsNullOrWhiteSpace(targetLang) &&
					!string.IsNullOrWhiteSpace(sourceLang))
				{
					translations = await GetFlashCards(context,
													sourceLang,
													targetLang,
													query => query
														.Where(t => t.SourceWord.Level.LevelName ==
														level));
				}
				break;
			case TypeOfQueryTranslation.Quantity:
				if (quantity > 0 &&
					!string.IsNullOrWhiteSpace(targetLang) &&
					!string.IsNullOrWhiteSpace(sourceLang))
				{
					translations = await GetFlashCards(context,
													sourceLang,
													targetLang,
													query => query
														.OrderBy(x => Guid.NewGuid())
														.Take(quantity));
				}
				break;
			default:
				throw new ArgumentException("Unsupported query type.", nameof(typeOfQueryTranslation));
		}

		return translations;
	}

	public static async Task<List<TranslationResponse>> Distribute(IMediator mediator,
																	TypeOfQueryTranslation typeOfQueryTranslation,
																	string sourceLanguage,
																	string targetLanguage,
																	string level = null,
																	int quantity = 0)
	{
		var translations = new List<TranslationResponse>();
		var reverseTranslations = new List<TranslationResponse>();

		switch (typeOfQueryTranslation)
		{
			case TypeOfQueryTranslation.Level:
				if (level != null)
				{
					translations = await mediator.Send(new GetTranslationsBy.Query
					{
						TypeOfQueryTranslation = TypeOfQueryTranslation.Level,
						Level = level,
						SourceLanguage = sourceLanguage,
						TargetLanguage = targetLanguage
					});

					reverseTranslations = await mediator.Send(new GetTranslationsBy.Query
					{
						TypeOfQueryTranslation = TypeOfQueryTranslation.Level,
						Level = level,
						SourceLanguage = targetLanguage,
						TargetLanguage = sourceLanguage
					});
				}
				break;
			case TypeOfQueryTranslation.Quantity:
				if (quantity > 0)
				{
					translations = await mediator.Send(new GetTranslationsBy.Query
					{
						TypeOfQueryTranslation = TypeOfQueryTranslation.Quantity,
						Quantity = quantity,
						SourceLanguage = sourceLanguage,
						TargetLanguage = targetLanguage
					});

					reverseTranslations = await mediator.Send(new GetTranslationsBy.Query
					{
						TypeOfQueryTranslation = TypeOfQueryTranslation.Quantity,
						Quantity = quantity,
						SourceLanguage = targetLanguage,
						TargetLanguage = sourceLanguage
					});
				}
				break;
			case TypeOfQueryTranslation.All:
				translations = await mediator.Send(new GetTranslationsBy.Query
				{
					TypeOfQueryTranslation = TypeOfQueryTranslation.All,
					SourceLanguage = sourceLanguage,
					TargetLanguage = targetLanguage
				});

				reverseTranslations = await mediator.Send(new GetTranslationsBy.Query
				{
					TypeOfQueryTranslation = TypeOfQueryTranslation.All,
					SourceLanguage = targetLanguage,
					TargetLanguage = sourceLanguage
				});
				break;
			default:
				throw new ArgumentException("Unsupported query type.", nameof(typeOfQueryTranslation));
		}

		var allTranslations = translations.Concat(reverseTranslations).ToList();

		var uniqueTranslations = new List<TranslationResponse>();
		foreach (var translation in allTranslations)
		{
			bool isDuplicate = uniqueTranslations.Any(t =>
				(t.SourceWord == translation.SourceWord && t.TargetWord == translation.TargetWord) ||
				(t.SourceWord == translation.TargetWord && t.TargetWord == translation.SourceWord));

			if (!isDuplicate)
			{
				uniqueTranslations.Add(translation);
			}
		}

		return uniqueTranslations;

	}
}