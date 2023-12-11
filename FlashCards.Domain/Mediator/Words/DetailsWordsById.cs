using AutoMapper;
using FlashCard.Model;
using FlashCard.Model.DTO.WordDto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Words;

public class DetailsWordsById
{
	public class Query : IRequest<WordResponse>
	{
		public Guid Id { get; set; }
	}
	public class Handler : IRequestHandler<Query, WordResponse>
	{
		private readonly FlashCardDbContext _context;
		private readonly IMapper _mapper;

		public Handler(FlashCardDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<WordResponse> Handle(Query request, CancellationToken cancellationToken)
		{
			var word = await _context.Words.Where(x => x.WordId == request.Id)
											.FirstOrDefaultAsync(cancellationToken);

			if (word == null)
			{
				throw new Exception("The word doesn't exist");
			}

			var wordResponse = _mapper.Map<WordResponse>(word);

			return wordResponse;
		}
	}
}