using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.Errors;
using Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;
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
			CompanyErrorCodes.CouldNotBeFound => NotFound(),
			ErrorCodes.ValidationError => BadRequest(result.Error.Description),
			_ => StatusCode(500)
		};
	}
}