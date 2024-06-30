using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Vehicles.DTOs;
using Car_Mender.Domain.Features.Vehicles.Errors;
using Car_Mender.Infrastructure.Features.Vehicles.Commands.CreateVehicle;
using Car_Mender.Infrastructure.Features.Vehicles.Commands.DeleteVehicle;
using Car_Mender.Infrastructure.Features.Vehicles.Commands.UpdateVehicle;
using Car_Mender.Infrastructure.Features.Vehicles.Queries;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Car_Mender.API.Features.Vehicles;

[ApiController]
[Route("api/vehicle")]
public class VehicleController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateVehicleAsync([FromBody] CreateVehicleCommand command)
	{
		var result = await mediator.Send(command);
		if (result.IsSuccess) return CreatedAtRoute("GetVehicleByIdRoute", new { id = result.Value }, result.Value);

		return result.Error.Code switch
		{
			VehicleErrorCodes.CouldNotBeFound => NotFound(result.Error.Description),
			ErrorCodes.ValidationError => BadRequest(result.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpGet("{id:guid}", Name = "GetVehicleByIdRoute")]
	public async Task<ActionResult<GetVehicleDto>> GetVehicleById(Guid id)
	{
		var query = new GetVehicleQuery(id);
		var getVehicleResult = await mediator.Send(query);
		if (getVehicleResult.IsSuccess) return Ok(getVehicleResult.Value);

		return getVehicleResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getVehicleResult.Error.Description),
			VehicleErrorCodes.CouldNotBeFound => NotFound(getVehicleResult.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpPatch("{id:guid}")]
	public async Task<IActionResult> UpdateVehicleById(Guid id,
		[FromBody] JsonPatchDocument<UpdateVehicleDto> patchDocument)
	{
		if (patchDocument is null) return BadRequest("Invalid Json Patch Document");

		var command = new UpdateVehicleCommand(id, patchDocument);
		var updateVehicleResult = await mediator.Send(command);
		if (updateVehicleResult.IsSuccess) return NoContent();

		return updateVehicleResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(updateVehicleResult.Error.Description),
			VehicleErrorCodes.CouldNotBeFound => NotFound(updateVehicleResult.Error.Description),
			_ => StatusCode(500, updateVehicleResult.Error.Description)
		};
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteVehicleById(Guid id)
	{
		var command = new DeleteVehicleCommand(id);
		var deleteVehicleResult = await mediator.Send(command);
		if (deleteVehicleResult.IsSuccess) return NoContent();

		return deleteVehicleResult.Error.Code switch
		{
			VehicleErrorCodes.CouldNotBeFound => NotFound(deleteVehicleResult.Error.Description),
			_ => StatusCode(500, deleteVehicleResult.Error.Description)
		};
	}
}