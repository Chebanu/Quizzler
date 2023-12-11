using AutoMapper;
using FlashCard.Model;
using FlashCard.Model.DTO.TranslationDto;
using FlashCard.Shared.Services.Translations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Translations;
/// <summary>
/// Create mediator class for translation
/// </summary>
public class CreateTranslations
{
	/// <summary>
	/// Command input properties
	/// </summary>
	public class Command : IRequest<Unit>
	{
		public IMediator Mediator { get; set; }
		public TranslationRequest TranslationRequest { get; set; }
	}

	public class Handler : IRequestHandler<Command, Unit>
	{
		private readonly FlashCardDbContext _context;

		public Handler(FlashCardDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Create translation
		/// </summary>
		/// <param name="request">Get properties from Command class</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Smth was uninitialize</exception>
		/// <exception cref="Exception">Could be thrown if source or target word do not exist at database.
		/// Or if the translation already exist at DB</exception>
		public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.TranslationRequest == null && request.Mediator == null)
				throw new ArgumentNullException($"{nameof(request)} or/and {nameof(request)} are empty");

			var sourceWord = await _context.Words.FirstOrDefaultAsync(x => x.WordText == request.TranslationRequest.SourceWord &&
															x.Language.LanguageName == request.TranslationRequest.SourceLanguage);

			var targetWord = await _context.Words.FirstOrDefaultAsync(x => x.WordText == request.TranslationRequest.TargetWord &&
															x.Language.LanguageName == request.TranslationRequest.TargetLanguage);

			if (sourceWord == null)
				throw new Exception("Source word does not exist at database");

			if (targetWord == null)
				throw new Exception("Target word does not exist at database");

			var newTranslation = new Translation
			{
				TranslationId = Guid.NewGuid(),
				SourceWordId = sourceWord.WordId,
				TargetWordId = targetWord.WordId
			};

			//Check if the translation exist at database(avoid dublicates)
			var isExist = await TranslationChecker.CheckIfTranslationExists(newTranslation, request.Mediator);

			if (isExist)
				throw new Exception("The translation is already exist");

			newTranslation.TranslationId = Guid.NewGuid();

			_context.Translations.Add(newTranslation);
			await _context.SaveChangesAsync();

			return Unit.Value;
		}
	}
}
