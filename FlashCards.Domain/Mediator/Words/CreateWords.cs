using AutoMapper;
using FlashCard.Model;
using FlashCard.Model.DTO.WordDto;
using FlashCard.Shared.Services.Words;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Words;

public class CreateWords
{
	public class Command : IRequest<Unit>
	{
		public IMediator Mediator { get; set; }
		public WordRequest WordRequest { get; set; }
	}

	public class Handler : IRequestHandler<Command, Unit>
	{
		private readonly FlashCardDbContext _context;
		private readonly IMapper _mapper;

		public Handler(FlashCardDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.WordRequest == null && request.Mediator == null)
				throw new Exception($"{nameof(request)} is empty");

			var word = _mapper.Map<Word>(request.WordRequest);

			var language = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageName ==
																request.WordRequest.Language.ToString());
			var level = await _context.Levels.FirstOrDefaultAsync(l => l.LevelName ==
																		request.WordRequest.Level.ToString());

			if (language == null || level == null)
				throw new ArgumentNullException("Something went wrong, 1 or more parameters don't match");

			word.LanguageId = language.LanguageId;
			word.LevelId = level.LevelId;

			var isExist = await WordChecker.CheckIfWordExists(word, request.Mediator);

			if (isExist)
				throw new Exception("The word is already exist");

			word.WordId = Guid.NewGuid();

			_context.Words.Add(word);
			await _context.SaveChangesAsync();

			return Unit.Value;
		}
	}
}
