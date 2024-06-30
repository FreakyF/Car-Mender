using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.DTOs;
using Car_Mender.Domain.Features.Appointments.Errors;
using Car_Mender.Infrastructure.Features.Appointments.Commands.CreateAppointment;
using Car_Mender.Infrastructure.Features.Appointments.Commands.DeleteAppointment;
using Car_Mender.Infrastructure.Features.Appointments.Commands.UpdateAppointment;
using Car_Mender.Infrastructure.Features.Appointments.Query;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Car_Mender.API.Features.Appointmnets;

[ApiController]
[Route("api/appointment")]
public class AppointmentController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateAppointmentAsync([FromBody] CreateAppointmentCommand command)
	{
		var result = await mediator.Send(command);
		if (result.IsSuccess) return CreatedAtRoute("GetAppointmentByIdRoute", new { id = result.Value }, result.Value);

		return result.Error.Code switch
		{
			AppointmentErrorCodes.CouldNotBeFound => NotFound(result.Error.Description),
			ErrorCodes.ValidationError => BadRequest(result.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpGet("{id:guid}", Name = "GetAppointmentByIdRoute")]
	public async Task<ActionResult<GetAppointmentDto>> GetCompanyById(Guid id)
	{
		var query = new GetAppointmentQuery(id);
		var getAppointmentResult = await mediator.Send(query);
		if (getAppointmentResult.IsSuccess) return Ok(getAppointmentResult.Value);

		return getAppointmentResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getAppointmentResult.Error.Description),
			AppointmentErrorCodes.CouldNotBeFound => NotFound(getAppointmentResult.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpPatch("{id:guid}")]
	public async Task<IActionResult> UpdateAppointmentById(Guid id,
		[FromBody] JsonPatchDocument<UpdateAppointmentDto> patchDocument)
	{
		if (patchDocument is null) return BadRequest("Invalid Json Patch Document");

		var command = new UpdateAppointmentCommand(id, patchDocument);
		var updateAppointmentResult = await mediator.Send(command);
		if (updateAppointmentResult.IsSuccess) return NoContent();

		return updateAppointmentResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(updateAppointmentResult.Error.Description),
			AppointmentErrorCodes.CouldNotBeFound => NotFound(updateAppointmentResult.Error.Description),
			_ => StatusCode(500, updateAppointmentResult.Error.Description)
		};
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAppointmentById(Guid id)
	{
		var command = new DeleteAppointmentCommand(id);
		var deleteAppointmentResult = await mediator.Send(command);
		if (deleteAppointmentResult.IsSuccess) return NoContent();

		return deleteAppointmentResult.Error.Code switch
		{
			AppointmentErrorCodes.CouldNotBeFound => NotFound(deleteAppointmentResult.Error.Description),
			_ => StatusCode(500, deleteAppointmentResult.Error.Description)
		};
	}
}