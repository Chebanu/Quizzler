using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlashCard.Controllers;

/// <summary>
/// BaseController for the entire solution
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class BaseApiController : ControllerBase
{
	private IMediator _mediator;


	/// <summary>
	/// Initialize Mediator for further using
	/// </summary>
	protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}
