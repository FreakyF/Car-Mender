using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Workers.DTOs;
using Car_Mender.Domain.Features.Workers.Errors;
using Car_Mender.Infrastructure.Features.Workers.Commands.CreateWorker;
using Car_Mender.Infrastructure.Features.Workers.Commands.DeleteWorker;
using Car_Mender.Infrastructure.Features.Workers.Commands.UpdateWorker;
using Car_Mender.Infrastructure.Features.Workers.Queries;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
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
		if (result.IsSuccess) return CreatedAtRoute("GetWorkerByIdRoute", new { id = result.Value }, result.Value);

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
		if (getWorkerResult.IsSuccess) return Ok(getWorkerResult.Value);

		return getWorkerResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getWorkerResult.Error.Description),
			WorkerErrorCodes.CouldNotBeFound => NotFound(getWorkerResult.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpPatch("{id:guid}")]
	public async Task<IActionResult> UpdateWorkerById(Guid id,
		[FromBody] JsonPatchDocument<UpdateWorkerDto> patchDocument)
	{
		if (patchDocument is null) return BadRequest("Invalid Json Patch Document");

		var command = new UpdateWorkerCommand(id, patchDocument);
		var updateWorkerResult = await mediator.Send(command);
		if (updateWorkerResult.IsSuccess) return NoContent();

		return updateWorkerResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(updateWorkerResult.Error.Description),
			WorkerErrorCodes.CouldNotBeFound => NotFound(updateWorkerResult.Error.Description),
			_ => StatusCode(500, updateWorkerResult.Error.Description)
		};
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteWorkerById(Guid id)
	{
		var command = new DeleteWorkerCommand(id);
		var deleteWorkerResult = await mediator.Send(command);
		if (deleteWorkerResult.IsSuccess) return NoContent();

		return deleteWorkerResult.Error.Code switch
		{
			WorkerErrorCodes.CouldNotBeFound => NotFound(deleteWorkerResult.Error.Description),
			_ => StatusCode(500, deleteWorkerResult.Error.Description)
		};
	}
}