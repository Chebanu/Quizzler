using AutoMapper;
using FlashCard.Model;
using FlashCard.Model.DTO.WordDto;
using FlashCard.Shared.Services.Words;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Mediator.Words;

public class EditWords
{
	public class Command : IRequest<WordResponse>
	{
		public IMediator Mediator { get; set; }
		public WordUpdateRequest WordUpdateRequest { get; set; }
	}

	public class Handler : IRequestHandler<Command, WordResponse>
	{
		private readonly FlashCardDbContext _context;
		private readonly IMapper _mapper;

		public Handler(FlashCardDbContext context, IMapper mapper = null)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<WordResponse> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.WordUpdateRequest == null || request.Mediator == null)
			{
				throw new Exception("Smth went wrong. 1 or more arguments are not initialize");
			}

			var getWord = await _context.Words.AsNoTracking()
											.FirstOrDefaultAsync(w => w.WordId == request.WordUpdateRequest.WordId);

			if (getWord == null)
			{
				throw new Exception("The word doesn't exist");
			}

			var word = _mapper.Map<Word>(request.WordUpdateRequest);

			var language = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageName ==
																request.WordUpdateRequest.Language.ToString());
			var level = await _context.Levels.FirstOrDefaultAsync(l => l.LevelName ==
																		request.WordUpdateRequest.Level.ToString());

			if (level == null || language == null)
			{
				throw new Exception("1 or more parameters don't exist in database");
			}

			word.LanguageId = language.LanguageId;
			word.LevelId = level.LevelId;

			var isExist = await WordChecker.CheckIfWordExists(word, request.Mediator);

			if (isExist)
				throw new Exception("The word is already exist");

			_context.Update(word);
			await _context.SaveChangesAsync();

			return _mapper.Map<WordResponse>(word);
		}
	}
}
