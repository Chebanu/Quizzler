/*using FlashCard.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Translations;

public class GetQuizTranslation
{
	public class Query : IRequest<List<Translation>>
	{
		public int Quantity { get; set; }
		public string SourceLanguage { get; set; }
		public string Targetlanguage { get; set; }
	}
	public class Handle : IRequestHandler<Query, List<Translation>>
	{
		private readonly FlashCardDbContext _context;

		public Handle(FlashCardDbContext context)
		{
			_context = context;
		}

		public async Task<List<Translation>> Handle(Query request, CancellationToken cancellationToken)
		{
			var quizTranlation = await _context.Translations.
		}

		private async Task<Word> GetRandomWord(string languageName, int quantity)
		{
			var words = await _context.Words
				.Where(w => w.Language.LanguageName == languageName)
				.OrderBy(x => Guid.NewGuid()) // Случайно перемешиваем слова.
				.Take(quantity)
				.FirstOrDefaultAsync();

			return words;
		}

	}
}

*/