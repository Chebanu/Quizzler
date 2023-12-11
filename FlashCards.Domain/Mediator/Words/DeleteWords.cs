using FlashCard.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Words;

public class DeleteWords
{
	public class Command : IRequest<Unit>
	{
		public Guid Id { get; set; }
	}

	public class Handler : IRequestHandler<Command, Unit>
	{
		private readonly FlashCardDbContext _context;
		public Handler(FlashCardDbContext context)
		{
			_context = context;
		}
		public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			var translation = await _context.Words.Where(p => p.WordId == request.Id)
												.FirstOrDefaultAsync(cancellationToken);

			if (translation == null)
			{
				throw new ArgumentNullException("The word doesn`t exist");
			}

			_context.Remove(translation);

			await _context.SaveChangesAsync();

			return Unit.Value;
		}
	}
}
