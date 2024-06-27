using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.DTOs;
using Car_Mender.Domain.Features.Branches.Errors;
using Car_Mender.Domain.Features.Companies.Errors;
using Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;
using Car_Mender.Infrastructure.Features.Branches.Commands.DeleteBranch;
using Car_Mender.Infrastructure.Features.Branches.Commands.UpdateBranch;
using Car_Mender.Infrastructure.Features.Branches.Queries;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Car_Mender.API.Features.Branches;

[ApiController]
[Route("api/branch")]
public class BranchController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateBranchAsync([FromBody] CreateBranchCommand command)
	{
		var result = await mediator.Send(command);
		if (result.IsSuccess) return CreatedAtRoute("GetBranchByIdRoute", new { id = result.Value }, result.Value);

		return result.Error.Code switch
		{
			BranchErrorCodes.CouldNotBeFound => NotFound(result.Error.Description),
			ErrorCodes.ValidationError => BadRequest(result.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpGet("{id:guid}", Name = "GetBranchByIdRoute")]
	public async Task<ActionResult<GetBranchDto>> GetCompanyById(Guid id)
	{
		var query = new GetBranchQuery(id);
		var getBranchResult = await mediator.Send(query);
		if (getBranchResult.IsSuccess) return Ok(getBranchResult.Value);

		return getBranchResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getBranchResult.Error.Description),
			BranchErrorCodes.CouldNotBeFound => NotFound(getBranchResult.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpPatch("{id:guid}")]
	public async Task<IActionResult> UpdateBranchById(Guid id,
		[FromBody] JsonPatchDocument<UpdateBranchDto> patchDocument)
	{
		if (patchDocument is null) return BadRequest("Invalid Json Patch Document");

		var command = new UpdateBranchCommand(id, patchDocument);
		var updateBranchResult = await mediator.Send(command);
		if (updateBranchResult.IsSuccess) return NoContent();

		return updateBranchResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(updateBranchResult.Error.Description),
			CompanyErrorCodes.CouldNotBeFound => NotFound(updateBranchResult.Error.Description),
			_ => StatusCode(500, updateBranchResult.Error.Description)
		};
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteCompanyById(Guid id)
	{
		var command = new DeleteBranchCommand(id);
		var deleteBranchResult = await mediator.Send(command);
		if (deleteBranchResult.IsSuccess) return NoContent();

		return deleteBranchResult.Error.Code switch
		{
			BranchErrorCodes.CouldNotBeFound => NotFound(deleteBranchResult.Error.Description),
			_ => StatusCode(500, deleteBranchResult.Error.Description)
		};
	}
}