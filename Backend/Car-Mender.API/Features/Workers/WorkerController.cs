using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Workers.DTOs;
using Car_Mender.Domain.Features.Workers.Errors;
using Car_Mender.Infrastructure.Features.Workers.Commands.CreateWorker;
using Car_Mender.Infrastructure.Features.Workers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Car_Mender.API.Features.Workers;

[ApiController]
[Route("api/worker")]
public class WorkerController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateWorkerAsync([FromBody] CreateWorkerCommand command)
	{
		var result = await mediator.Send(command);
		if (result.IsSuccess)
		{
			return CreatedAtRoute("GetWorkerByIdRoute", new { id = result.Value }, result.Value);
		}

		return result.Error.Code switch
		{
			WorkerErrorCodes.CouldNotBeFound => NotFound(result.Error.Description),
			ErrorCodes.ValidationError => BadRequest(result.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpGet("{id:guid}", Name = "GetWorkerByIdRoute")]
	public async Task<ActionResult<GetWorkerDto>> GetWorkerById(Guid id)
	{
		var query = new GetWorkerQuery(id);
		var getWorkerResult = await mediator.Send(query);
		if (getWorkerResult.IsSuccess)
		{
			return Ok(getWorkerResult.Value);
		}

		return getWorkerResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getWorkerResult.Error.Description),
			WorkerErrorCodes.CouldNotBeFound => NotFound(getWorkerResult.Error.Description),
			_ => StatusCode(500)
		};
	}
}