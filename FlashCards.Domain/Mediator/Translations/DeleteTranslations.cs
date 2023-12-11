using FlashCard.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Translations;

/// <summary>
/// Delete mediator class for translation
/// </summary>
public class DeleteTranslations
{
	/// <summary>
	/// Command input properties
	/// </summary>
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

		/// <summary>
		/// Delete translation from db
		/// </summary>
		/// <param name="request">Get properties from Command class</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			var translation = await _context.Translations.Where(x => x.TranslationId == request.Id)
														.FirstOrDefaultAsync(cancellationToken);

			if (translation == null)
			{
				throw new Exception("The translation is not found");
			}

			_context.Remove(translation);

			await _context.SaveChangesAsync();

			return Unit.Value;
		}
	}
}
