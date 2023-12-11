using Microsoft.AspNetCore.Mvc;
using FlashCard.Mediator.Translations;
using FlashCard.Shared.Services.Translations;
using FlashCard.Model.DTO.TranslationDto;

namespace FlashCard.Controllers;

/// <summary>
/// Translation Controller manipulate with translation instances
/// </summary>
public class TranslationsController : BaseApiController
{
	/// <summary>
	/// Get list of translations sorting by source and target languages
	/// </summary>
	/// <param name="sourceLanguage">The language you currently learn</param>
	/// <param name="targetLanguage">The language which you already know</param>
	/// <returns></returns>
	[HttpGet]
	public async Task<ActionResult<List<TranslationResponse>>> GetTranslations(string sourceLanguage, string targetLanguage)
	{
		if (sourceLanguage == targetLanguage)
			return BadRequest("Source and target languages must be different");

		var uniqueTranslations = await TranslationDistributor.Distribute(Mediator,
																			TypeOfQueryTranslation.All,
																			sourceLanguage,
																			targetLanguage);

		return Ok(uniqueTranslations);
	}

	/// <summary>
	/// Get 1 translation by Guid
	/// </summary>
	/// <param name="id">Translation id property</param>
	/// <returns></returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<TranslationResponse>> GetTranslation(Guid id)
	{
		try
		{
			return await Mediator.Send(new DetailsTranslation.Query { Id = id });
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	/// <summary>
	/// Get certain amount of translations by source and target language
	/// </summary>
	/// <param name="quantity">Define the quantity of cards you'll from db</param>
	/// <param name="sourceLanguage">The language you currently learn</param>
	/// <param name="targetLanguage">The language which you already know</param>
	/// <returns>Certain amount of flash cards</returns>
	[HttpGet("quantity/{quantity}/{sourceLanguage}/{targetLanguage}")]
	public async Task<ActionResult<List<TranslationResponse>>> GetRandomQuantityOfCards(int quantity, string sourceLanguage, string targetLanguage)
	{
		if (quantity <= 0 || sourceLanguage == targetLanguage)
			return BadRequest("Something went wrong");

		var uniqueTranslations = await TranslationDistributor.Distribute(Mediator,
																			TypeOfQueryTranslation.Quantity,
																			sourceLanguage,
																			targetLanguage,
																			quantity: quantity);

		return Ok(uniqueTranslations.Take(quantity));
	}

	/// <summary>
	/// Get translations by their level. Sorted by source and target languages
	/// </summary>
	/// <param name="level">Define the level of source word(A1,A2,B1 etc.)</param>
	/// <param name="sourceLanguage">The language you currently learn</param>
	/// <param name="targetLanguage">The language which you already know</param>
	/// <returns></returns>
	[HttpGet("level/{level}/{sourceLanguage}/{targetLanguage}")]
	public async Task<ActionResult<List<TranslationResponse>>> GetCardsByLevel(string level, string sourceLanguage, string targetLanguage)
	{
		if (sourceLanguage == targetLanguage)
			return BadRequest("Source and target languages must be different");

		var uniqueTranslations = await TranslationDistributor.Distribute(Mediator,
																			TypeOfQueryTranslation.Level,
																			sourceLanguage,
																			targetLanguage,
																			level: level);

		return uniqueTranslations;
	}

	/// <summary>
	/// Create a translation
	/// </summary>
	/// <param name="translationRequest">Dto for translation request</param>
	/// <returns></returns>
	[HttpPost]
	public async Task<ActionResult> Create(TranslationRequest translationRequest)
	{
		if (translationRequest.SourceLanguage == translationRequest.TargetLanguage)
			return BadRequest("You can not add translation for the same language as source one");

		try
		{
			await Mediator.Send(new CreateTranslations.Command
			{
				TranslationRequest = translationRequest,
				Mediator = Mediator
			});
		}
		catch (ArgumentNullException ex)
		{
			return BadRequest($"Argument exception, {ex}");
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}

		return Ok();
	}


	/// <summary>
	/// Edit translation
	/// </summary>
	/// <param name="translationUpdateRequest"></param>
	/// <returns></returns>
	[HttpPut]
	public async Task<ActionResult<TranslationResponse>> Edit(TranslationUpdateRequest translationUpdateRequest)
	{
		if (translationUpdateRequest.SourceWord == translationUpdateRequest.TargetWord &&
			translationUpdateRequest.SourceLanguage == translationUpdateRequest.TargetLanguage)
			return BadRequest("You can not edit translation for the same language as source one");

		var newTranslation = new TranslationResponse();

		try
		{
			newTranslation = await Mediator.Send(new EditTranslations.Command
			{
				TranslationUpdateRequest = translationUpdateRequest,
				Mediator = Mediator
			});
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}

		return newTranslation;
	}


	/// <summary>
	/// Delete translation
	/// </summary>
	/// <param name="id">Delete by id</param>
	/// <returns></returns>
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		try
		{
			await Mediator.Send(new DeleteTranslations.Command { Id = id });
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}

		return Ok();
	}
}