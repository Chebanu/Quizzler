using AutoMapper;
using FlashCard.Model;
using FlashCard.Model.DTO.TranslationDto;
using FlashCard.Shared.Services.Translations;
using MediatR;

namespace FlashCard.Mediator.Translations;

public class GetTranslationsBy
{
	public class Query : IRequest<List<TranslationResponse>>
	{
		public TypeOfQueryTranslation TypeOfQueryTranslation { get; set; }
		public string Level { get; set; }
		public string SourceLanguage { get; set; }
		public string TargetLanguage { get; set; }
		public int Quantity { get; set; }
	}
	public class Handler : IRequestHandler<Query, List<TranslationResponse>>
	{
		private readonly FlashCardDbContext _context;
		private readonly IMapper _mapper;

		public Handler(FlashCardDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public async Task<List<TranslationResponse>> Handle(Query request, CancellationToken cancellationToken)
		{
			var translations = new List<Translation>();

			switch (request.TypeOfQueryTranslation)
			{
				case TypeOfQueryTranslation.All:
					translations = await TranslationDistributor.GetFalshCardsByParameters(_context,
															request.TypeOfQueryTranslation,
															request.SourceLanguage,
															request.TargetLanguage);
					break;
				case TypeOfQueryTranslation.Level:
					translations = await TranslationDistributor.GetFalshCardsByParameters(_context,
															request.TypeOfQueryTranslation,
															request.SourceLanguage,
															request.TargetLanguage,
															level: request.Level);
					break;
				case TypeOfQueryTranslation.Quantity:
					translations = await TranslationDistributor.GetFalshCardsByParameters(_context,
															request.TypeOfQueryTranslation,
															request.SourceLanguage,
															request.TargetLanguage,
															quantity: request.Quantity);
					break;
				default:
					break;
			}

			var translationResponse = _mapper.Map<List<TranslationResponse>>(translations);

			return translationResponse;
		}
	}
}