using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.DTOs;
using Car_Mender.Domain.Features.Branches.Errors;
using Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;
using Car_Mender.Infrastructure.Features.Branches.Queries;
using MediatR;
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
		if (result.IsSuccess)
		{
			return CreatedAtRoute("GetBranchByIdRoute", new { id = result.Value }, result.Value);
		}

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
		if (getBranchResult.IsSuccess)
		{
			return Ok(getBranchResult.Value);
		}

		return getBranchResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getBranchResult.Error.Description),
			BranchErrorCodes.CouldNotBeFound => NotFound(getBranchResult.Error.Description),
			_ => StatusCode(500)
		};
	}
}