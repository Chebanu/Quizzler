using FlashCard.Mediator.Words;
using MediatR;

namespace FlashCard.Shared.Services.Words;

public class WordChecker
{
	public static async Task<bool> CheckIfWordExists(Word word, IMediator mediator)
	{
		var isWordExist = await mediator.Send(new IsWordExist.Query
		{
			WordText = word.WordText,
			LanguageId = word.LanguageId
		});

		if (isWordExist)
			return true;

		return false;
	}
}