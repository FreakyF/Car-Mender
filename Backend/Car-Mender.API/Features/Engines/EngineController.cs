using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.DTOs;
using Car_Mender.Domain.Features.Engines.Errors;
using Car_Mender.Infrastructure.Features.Engines.Commands.CreateEngine;
using Car_Mender.Infrastructure.Features.Engines.Commands.DeleteEngine;
using Car_Mender.Infrastructure.Features.Engines.Commands.UpdateEngine;
using Car_Mender.Infrastructure.Features.Engines.Queries.GetEngineQuery;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Car_Mender.API.Features.Engines;

[ApiController]
[Route("api/engine")]
public class EngineController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateEngineAsync([FromBody] CreateEngineCommand command)
	{
		var result = await mediator.Send(command);
		if (result.IsSuccess) return CreatedAtRoute("GetEngineByIdRoute", new { id = result.Value }, result.Value);

		return result.Error.Code switch
		{
			EngineErrorCodes.CouldNotBeFound => NotFound(result.Error.Description),
			ErrorCodes.ValidationError => BadRequest(result.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpGet("{id:guid}", Name = "GetEngineByIdRoute")]
	public async Task<ActionResult<GetEngineDto>> GetEngineById(Guid id)
	{
		var query = new GetEngineQuery(id);
		var getEngineResult = await mediator.Send(query);
		if (getEngineResult.IsSuccess) return Ok(getEngineResult.Value);

		return getEngineResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getEngineResult.Error.Description),
			EngineErrorCodes.CouldNotBeFound => NotFound(getEngineResult.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpPatch("{id:guid}")]
	public async Task<IActionResult> UpdateEngineById(Guid id,
		[FromBody] JsonPatchDocument<UpdateEngineDto> patchDocument)
	{
		if (patchDocument is null) return BadRequest("Invalid Json Patch Document");

		var command = new UpdateEngineCommand(id, patchDocument);
		var updateEngineResult = await mediator.Send(command);
		if (updateEngineResult.IsSuccess) return NoContent();

		return updateEngineResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(updateEngineResult.Error.Description),
			EngineErrorCodes.CouldNotBeFound => NotFound(updateEngineResult.Error.Description),
			_ => StatusCode(500, updateEngineResult.Error.Description)
		};
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteEngineById(Guid id)
	{
		var command = new DeleteEngineCommand(id);
		var deleteEngineResult = await mediator.Send(command);
		if (deleteEngineResult.IsSuccess) return NoContent();

		return deleteEngineResult.Error.Code switch
		{
			EngineErrorCodes.CouldNotBeFound => NotFound(deleteEngineResult.Error.Description),
			_ => StatusCode(500, deleteEngineResult.Error.Description)
		};
	}
}