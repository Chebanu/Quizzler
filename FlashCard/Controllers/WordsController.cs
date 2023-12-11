using FlashCard.Mediator.Words;
using FlashCard.Model.DTO.WordDto;
using FlashCard.Shared.Services.Translations;
using Microsoft.AspNetCore.Mvc;

namespace FlashCard.Controllers;

/// <summary>
/// Word Controller
/// </summary>
public class WordsController : BaseApiController
{
	/// <summary>
	/// Get all words from DB
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<ActionResult<List<WordResponse>>> GetWords()
	{
		return await Mediator.Send(new GetWordsBy.Query
		{
			TypeOfQueryWord = TypeOfQueryWord.All
		});
	}


	/// <summary>
	/// Get all words by language
	/// </summary>
	/// <param name="targetLanguage">Get words by the language you want</param>
	/// <returns>List of words</returns>
	[HttpGet("byLanguage/{targetLanguage}")]
	public async Task<ActionResult<List<WordResponse>>> GetWordsByLanguage(string targetLanguage)
	{
		return await Mediator.Send(new GetWordsBy.Query
		{
			TypeOfQueryWord = TypeOfQueryWord.Language,
			TargetLanguage = targetLanguage
		});
	}


	/// <summary>
	/// Get a certain amount of words by a language
	/// </summary>
	/// <param name="targetLanguage">language, you want to get cards</param>
	/// <param name="quantity">the amount of the words</param>
	/// <returns>List of a certain amount of Words</returns>
	[HttpGet("byQuantity/{quantity}/{targetLanguage}")]
	public async Task<ActionResult<List<WordResponse>>> GetWordsByQuantity(string targetLanguage, int quantity)
	{
		//add verification for existing of the lang
		//upd, verification will not be implemented, will be a filter on client lvl
		return await Mediator.Send(new GetWordsBy.Query
		{
			TypeOfQueryWord = TypeOfQueryWord.Quantity,
			TargetLanguage = targetLanguage,
			Quantity = quantity
		});
	}

	/// <summary>
	/// Get a word by id
	/// </summary>
	/// <param name="id">id of the word</param>
	/// <returns></returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<WordResponse>> GetWord(Guid id)
	{
		var word = new WordResponse();

		try
		{
			word = await Mediator.Send(new DetailsWordsById.Query { Id = id });
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}

		return Ok(word);
	}

	/// <summary>
	/// Create a word
	/// </summary>
	/// <param name="wordRequest">Dto for word request</param>
	/// <returns></returns>
	[HttpPost]
	public async Task<ActionResult> Create(WordRequest wordRequest)
	{
		try
		{
			await Mediator.Send(new CreateWords.Command
			{
				WordRequest = wordRequest,
				Mediator = Mediator
			});
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}

		return Ok();
	}

	/// <summary>
	/// Edit the word
	/// </summary>
	/// <param name="wordUpdateRequest">Dto for word updation</param>
	/// <returns></returns>
	[HttpPut]
	public async Task<ActionResult> Edit(WordUpdateRequest wordUpdateRequest)
	{
		return Ok(await Mediator.Send(new EditWords.Command { WordUpdateRequest = wordUpdateRequest, Mediator = Mediator }));
	}

	/// <summary>
	/// Delete a word by id
	/// </summary>
	/// <param name="id">Id of the word</param>
	/// <returns></returns>
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		try
		{
			await Mediator.Send(new DeleteWords.Command { Id = id });
		}
		catch (ArgumentNullException ex)
		{
			return BadRequest(ex.Message);
		}

		return Ok();
	}
}