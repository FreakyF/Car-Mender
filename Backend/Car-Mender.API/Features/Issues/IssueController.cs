using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Issues.DTOs;
using Car_Mender.Domain.Features.Issues.Errors;
using Car_Mender.Infrastructure.Features.Issues.Commands.CreateIssue;
using Car_Mender.Infrastructure.Features.Issues.Commands.DeleteIssue;
using Car_Mender.Infrastructure.Features.Issues.Commands.UpdateIssue;
using Car_Mender.Infrastructure.Features.Issues.Queries;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Car_Mender.API.Features.Issues;

[ApiController]
[Route("api/issue")]
public class IssueController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateIssueAsync([FromBody] CreateIssueCommand command)
	{
		var result = await mediator.Send(command);
		if (result.IsSuccess) return CreatedAtRoute("GetIssueByIdRoute", new { id = result.Value }, result.Value);

		return result.Error.Code switch
		{
			IssueErrorCodes.CouldNotBeFound => NotFound(result.Error.Description),
			ErrorCodes.ValidationError => BadRequest(result.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpGet("{id:guid}", Name = "GetIssueByIdRoute")]
	public async Task<ActionResult<GetIssueDto>> GetWorkerById(Guid id)
	{
		var query = new GetIssueQuery(id);
		var getIssueResult = await mediator.Send(query);
		if (getIssueResult.IsSuccess) return Ok(getIssueResult.Value);

		return getIssueResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getIssueResult.Error.Description),
			IssueErrorCodes.CouldNotBeFound => NotFound(getIssueResult.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpPatch("{id:guid}")]
	public async Task<IActionResult> UpdateIssueById(Guid id,
		[FromBody] JsonPatchDocument<UpdateIssueDto>? patchDocument)
	{
		if (patchDocument is null) return BadRequest("Invalid Json Patch Document");

		var command = new UpdateIssueCommand(id, patchDocument);
		var updateIssueResult = await mediator.Send(command);
		if (updateIssueResult.IsSuccess) return NoContent();

		return updateIssueResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(updateIssueResult.Error.Description),
			IssueErrorCodes.CouldNotBeFound => NotFound(updateIssueResult.Error.Description),
			_ => StatusCode(500, updateIssueResult.Error.Description)
		};
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteIssueById(Guid id)
	{
		var command = new DeleteIssueCommand(id);
		var deleteIssueResult = await mediator.Send(command);
		if (deleteIssueResult.IsSuccess) return NoContent();

		return deleteIssueResult.Error.Code switch
		{
			IssueErrorCodes.CouldNotBeFound => NotFound(deleteIssueResult.Error.Description),
			_ => StatusCode(500, deleteIssueResult.Error.Description)
		};
	}
}