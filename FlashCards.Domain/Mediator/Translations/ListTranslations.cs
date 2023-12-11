/*using AutoMapper;
using FlashCard.Model;
using FlashCard.Model.DTO.TranslationDto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Translations;

public class ListTranslations
{
	public class Query : IRequest<List<TranslationResponse>>
	{
		public string SourceLanguage { get; set; }
		public string TargetLanguage { get; set; }
	}

	public class Handler : IRequestHandler<Query, List<TranslationResponse>>
	{
		public readonly FlashCardDbContext _context;
		private readonly IMapper _mapper;

		public Handler(FlashCardDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<TranslationResponse>> Handle(Query request, CancellationToken cancellationToken)
		{
			var translation = await _context.Translations.Include(t => t.SourceWord)
												.Include(l => l.SourceWord.Level)
												.Include(l => l.SourceWord.Language)
												.Include(t => t.TargetWord)
												.Include(l => l.TargetWord.Level)
												.Include(l => l.TargetWord.Language)
												.Where(t => t.SourceWord.Language.LanguageName ==
														request.SourceLanguage &&
														t.TargetWord.Language.LanguageName ==
														request.TargetLanguage)
											.ToListAsync();

			 var responseTranslation = _mapper.Map<List<TranslationResponse>>(translation);

			return responseTranslation;
		}
	}
}
*/