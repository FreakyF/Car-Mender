using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Car_Mender.Infrastructure.Features.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandHandler(
	ICompanyRepository companyRepository,
	ILogger<DeleteCompanyCommandHandler> logger)
	: IRequestHandler<DeleteCompanyCommand, Result>
{
	public async Task<Result> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty))
		{
			return Error.InvalidId;
		}
		
		var deleteCompanyResult = await companyRepository.DeleteCompanyAsync(request.Id, cancellationToken);
		if (deleteCompanyResult.IsFailure)
		{
			logger.LogError("Could not remove company with id: {Id}: {Message}", request.Id,
				deleteCompanyResult.Error.Description);
		}

		return deleteCompanyResult;
	}
}