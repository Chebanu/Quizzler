using AutoMapper;
using FlashCard.Model;
using FlashCard.Model.DTO.TranslationDto;
using FlashCard.Shared.Services.Translations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Translations;

public class EditTranslations
{
	public class Command : IRequest<TranslationResponse>
	{
		public IMediator Mediator { get; set; }
		public TranslationUpdateRequest TranslationUpdateRequest { get; set; }
	}

	public class Handler : IRequestHandler<Command, TranslationResponse>
	{
		private readonly FlashCardDbContext _context;
		private readonly IMapper _mapper;

		public Handler(FlashCardDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<TranslationResponse> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.TranslationUpdateRequest == null || request.Mediator == null)
				throw new Exception("Smth went wrong. 1 or more arguments are not initialize");

			var getTranslation = await _context.Translations.AsNoTracking()
											.FirstOrDefaultAsync(w => w.TranslationId ==
												request.TranslationUpdateRequest.TranslationId);

			if (getTranslation == null)
				throw new Exception("The translation doesn't exist");

			//get source word from db

			var sourceWord = await _context.Words
												.Include(s => s.Language)
												.Include(s => s.Level)
											.Select(x => new Word
											{
												WordText = x.WordText,
												Language = x.Language,
												Level = x.Level,
												ImageUrl = x.ImageUrl,
											})
											.Where(x => x.WordText == request.TranslationUpdateRequest.SourceWord)
											.Where(x => x.Language.LanguageName == request.TranslationUpdateRequest.SourceLanguage)
										.FirstOrDefaultAsync();

			//get target word from db

			var targetWord = await _context.Words
												.Include(s => s.Language)
												.Include(s => s.Level)
											.Select(x => new Word
											{
												WordText = x.WordText,
												Language = x.Language,
												Level = x.Level,
												ImageUrl = x.ImageUrl,
											})
											.Where(x => x.WordText == request.TranslationUpdateRequest.TargetWord)
											.Where(x => x.Language.LanguageName == request.TranslationUpdateRequest.TargetLanguage)
										.FirstOrDefaultAsync();

			if (sourceWord == null)
				throw new Exception("Source word does not exist at database");

			if (targetWord == null)
				throw new Exception("Target word does not exist at database");

			var translation = new Translation
			{
				TranslationId = request.TranslationUpdateRequest.TranslationId,
				SourceWord = sourceWord,
				TargetWord = targetWord,
			};

			var isExist = await TranslationChecker.CheckIfTranslationExists(translation, request.Mediator);

			if (isExist)
				throw new Exception("Translation can not be the same as previous one");

			_context.Update(translation);
			await _context.SaveChangesAsync();

			return new TranslationResponse
			{
				TranslationId = translation.TranslationId,
				SourceWord = sourceWord.WordText,
				TargetWord = targetWord.WordText,
				SourceLanguageName = sourceWord.Language.LanguageName,
				TargetLanguageName = targetWord.Language.LanguageName,
				SourceLevelName = sourceWord.Level.LevelName,
				Image = sourceWord.ImageUrl
			};
		}
	}
}
