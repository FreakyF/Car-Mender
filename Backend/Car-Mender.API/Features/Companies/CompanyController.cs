using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Errors;
using Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;
using Car_Mender.Infrastructure.Features.Companies.Queries.GetCompanyQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Car_Mender.API.Features.Companies;

[ApiController]
[Route("api/company")]
public class CompanyController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateCompanyAsync(CreateCompanyCommand command)
	{
		var result = await mediator.Send(command);
		if (result.IsSuccess)
		{
			return NoContent();
		}

		return result.Error.Code switch
		{
			CompanyErrorCodes.CouldNotBeFound => NotFound(result.Error.Description),
			ErrorCodes.ValidationError => BadRequest(result.Error.Description),
			_ => StatusCode(500)
		};
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<GetCompanyDto>> GetCompanyAsync(Guid id)
	{
		var query = new GetCompanyQuery(id);
		var getCompanyResult = await mediator.Send(query);
		if (getCompanyResult.IsSuccess)
		{
			return Ok(getCompanyResult.Value);
		}

		return getCompanyResult.Error.Code switch
		{
			ErrorCodes.InvalidId => BadRequest(getCompanyResult.Error.Description),
			CompanyErrorCodes.CouldNotBeFound => NotFound(getCompanyResult.Error.Description),
			_ => StatusCode(500)
		};
	}
}