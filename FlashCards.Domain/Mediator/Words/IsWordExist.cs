using FlashCard.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Words;

public class IsWordExist
{
	public class Query : IRequest<bool>
	{
		public string WordText { get; set; }
		public Guid LanguageId { get; set; }
	}
	public class Handler : IRequestHandler<Query, bool>
	{
		private readonly FlashCardDbContext _context;

		public Handler(FlashCardDbContext context)
		{
			_context = context;
		}

		public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
		{
			return await _context.Words.AnyAsync(w => w.WordText == request.WordText && w.LanguageId == request.LanguageId);
		}
	}
}