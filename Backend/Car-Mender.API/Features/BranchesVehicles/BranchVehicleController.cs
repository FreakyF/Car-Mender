using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.BranchesVehicles.DTOs;
using Car_Mender.Domain.Features.BranchesVehicles.Errors;
using Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.CreateBranchVehicle;
using Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.DeleteBranchVehicle;
using Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.UpdateBranchVehicle;
using Car_Mender.Infrastructure.Features.BranchesVehicles.Queries;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Car_Mender.API.Features.BranchesVehicles;

[ApiController]
[Route("api/branchVehicle")]
public class BranchVehicleController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateBranchVehicleAsync([FromBody] CreateBranchVehicleCommand command)
	{
		var result = await mediator.Send(command);
		if (result.IsSuccess)
			return CreatedAtRoute("GetBranchVehicleByIdRoute", new { id = result.Value }, result.Value);

		return result.Error.Code switch
		{
			BranchVehicleErrorCodes.CouldNotBeFound => NotFound(result.Error.Description),
			ErrorCodes.ValidationError => BadRequest(result.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpGet("{id:guid}", Name = "GetBranchVehicleByIdRoute")]
	public async Task<ActionResult<GetBranchVehicleDto>> GetCompanyById(Guid id)
	{
		var query = new GetBranchVehicleQuery(id);
		var getBranchVehicleResult = await mediator.Send(query);
		if (getBranchVehicleResult.IsSuccess) return Ok(getBranchVehicleResult.Value);

		return getBranchVehicleResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getBranchVehicleResult.Error.Description),
			BranchVehicleErrorCodes.CouldNotBeFound => NotFound(getBranchVehicleResult.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpPatch("{id:guid}")]
	public async Task<IActionResult> UpdateBranchVehicleById(Guid id,
		[FromBody] JsonPatchDocument<UpdateBranchVehicleDto> patchDocument)
	{
		if (patchDocument is null) return BadRequest("Invalid Json Patch Document");

		var command = new UpdateBranchVehicleCommand(id, patchDocument);
		var updateBranchVehicleResult = await mediator.Send(command);
		if (updateBranchVehicleResult.IsSuccess) return NoContent();

		return updateBranchVehicleResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(updateBranchVehicleResult.Error.Description),
			BranchVehicleErrorCodes.CouldNotBeFound => NotFound(updateBranchVehicleResult.Error.Description),
			_ => StatusCode(500, updateBranchVehicleResult.Error.Description)
		};
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteBranchVehicleById(Guid id)
	{
		var command = new DeleteBranchVehicleCommand(id);
		var deleteBranchVehicleResult = await mediator.Send(command);
		if (deleteBranchVehicleResult.IsSuccess) return NoContent();

		return deleteBranchVehicleResult.Error.Code switch
		{
			BranchVehicleErrorCodes.CouldNotBeFound => NotFound(deleteBranchVehicleResult.Error.Description),
			_ => StatusCode(500, deleteBranchVehicleResult.Error.Description)
		};
	}
}