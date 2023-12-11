using FlashCard.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Words;

public class IsTranslationExist
{
	public class Query : IRequest<bool>
	{
		public Guid SourceId { get; set; }
		public Guid TargetId { get; set; }
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
			return await _context.Translations.AnyAsync(w => w.SourceWordId == request.SourceId && w.TargetWordId == request.TargetId);
		}
	}
}