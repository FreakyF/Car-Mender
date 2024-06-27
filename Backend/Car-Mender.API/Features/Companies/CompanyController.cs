using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Errors;
using Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;
using Car_Mender.Infrastructure.Features.Companies.Commands.DeleteCompany;
using Car_Mender.Infrastructure.Features.Companies.Commands.UpdateCompany;
using Car_Mender.Infrastructure.Features.Companies.Queries.GetCompanyQuery;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Car_Mender.API.Features.Companies;

[ApiController]
[Route("api/company")]
public class CompanyController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyCommand command)
	{
		var result = await mediator.Send(command);
		if (result.IsSuccess) return CreatedAtRoute("GetCompanyByIdRoute", new { id = result.Value }, result.Value);

		return result.Error.Code switch
		{
			CompanyErrorCodes.CouldNotBeFound => NotFound(result.Error.Description),
			ErrorCodes.ValidationError => BadRequest(result.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpGet("{id:guid}", Name = "GetCompanyByIdRoute")]
	public async Task<ActionResult<GetCompanyDto>> GetCompanyById(Guid id)
	{
		var query = new GetCompanyQuery(id);
		var getCompanyResult = await mediator.Send(query);
		if (getCompanyResult.IsSuccess) return Ok(getCompanyResult.Value);

		return getCompanyResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getCompanyResult.Error.Description),
			CompanyErrorCodes.CouldNotBeFound => NotFound(getCompanyResult.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpPatch("{id:guid}")]
	public async Task<IActionResult> UpdateCompanyById(Guid id,
		[FromBody] JsonPatchDocument<UpdateCompanyDto> patchDocument)
	{
		if (patchDocument is null) return BadRequest("Invalid Json Patch Document");

		var command = new UpdateCompanyCommand(id, patchDocument);
		var updateCompanyResult = await mediator.Send(command);
		if (updateCompanyResult.IsSuccess) return NoContent();

		return updateCompanyResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(updateCompanyResult.Error.Description),
			CompanyErrorCodes.CouldNotBeFound => NotFound(updateCompanyResult.Error.Description),
			_ => StatusCode(500, updateCompanyResult.Error.Description)
		};
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteCompanyById(Guid id)
	{
		var command = new DeleteCompanyCommand(id);
		var deleteCompanyResult = await mediator.Send(command);
		if (deleteCompanyResult.IsSuccess) return NoContent();

		return deleteCompanyResult.Error.Code switch
		{
			CompanyErrorCodes.CouldNotBeFound => NotFound(deleteCompanyResult.Error.Description),
			_ => StatusCode(500, deleteCompanyResult.Error.Description)
		};
	}
}