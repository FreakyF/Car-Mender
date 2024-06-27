using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.Repository;
using Car_Mender.Infrastructure.Features.Branches.Commands.DeleteBranch;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Car_Mender.Infrastructure.Features.Engines.Commands.DeleteEngine;

public class DeleteEngineCommandHandler(
	IEngineRepository engineRepository,
	ILogger<DeleteBranchCommandHandler> logger)
	: IRequestHandler<DeleteEngineCommand, Result>
{
	public async Task<Result> Handle(DeleteEngineCommand request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty))
		{
			return Error.InvalidId;
		}

		var deleteEngineResult = await engineRepository.DeleteEngineAsync(request.Id, cancellationToken);
		if (deleteEngineResult.IsFailure)
		{
			logger.LogError("Could not remove engine with id: {Id}: {Message}", request.Id,
				deleteEngineResult.Error.Description);
		}

		return deleteEngineResult;
	}
}