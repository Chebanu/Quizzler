using FlashCard.Mediator.Words;
using MediatR;

namespace FlashCard.Shared.Services.Translations;

public class TranslationChecker
{
	public static async Task<bool> CheckIfTranslationExists(Translation translation, IMediator mediator)
	{
		var isTranslationExist = await mediator.Send(new IsTranslationExist.Query
		{
			SourceId = translation.SourceWordId,
			TargetId = translation.TargetWordId
		});

		if (!isTranslationExist)
		{
			isTranslationExist = await mediator.Send(new IsTranslationExist.Query
			{
				SourceId = translation.TargetWordId,
				TargetId = translation.SourceWordId
			});
		}

		return isTranslationExist;
	}
}
