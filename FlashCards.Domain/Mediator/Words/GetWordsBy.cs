using AutoMapper;
using FlashCard.Model;
using FlashCard.Model.DTO.WordDto;
using FlashCard.Shared.Services.Translations;
using MediatR;

namespace FlashCard.Mediator.Words;

public class GetWordsBy
{
	public class Query : IRequest<List<WordResponse>>
	{
		public TypeOfQueryWord TypeOfQueryWord { get; set; }
		public string TargetLanguage { get; set; }
		public string Level { get; set; }
		public int Quantity { get; set; }
	}

	public class Handler : IRequestHandler<Query, List<WordResponse>>
	{
		public readonly FlashCardDbContext _context;
		private readonly IMapper _mapper;

		public Handler(FlashCardDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<WordResponse>> Handle(Query request, CancellationToken cancellationToken)
		{
			var words = new List<Word>();

			switch (request.TypeOfQueryWord)
			{
				case TypeOfQueryWord.All:
					words = await GetWords.GetFalshCardsByParameters(_context,
																	request.TypeOfQueryWord);
					break;
				case TypeOfQueryWord.Level:
					words = await GetWords.GetFalshCardsByParameters(_context,
																	request.TypeOfQueryWord,
																	request.TargetLanguage,
																	request.Level);
					break;
				case TypeOfQueryWord.Quantity:
					words = await GetWords.GetFalshCardsByParameters(_context,
																	request.TypeOfQueryWord,
																	request.TargetLanguage,
																	quantity: request.Quantity);
					break;
				case TypeOfQueryWord.Language:
					words = await GetWords.GetFalshCardsByParameters(_context,
																	request.TypeOfQueryWord,
																	request.TargetLanguage);
					break;
				default:
					break;
			}

			var wordResponse = _mapper.Map<List<WordResponse>>(words);

			return wordResponse;
		}
	}
}