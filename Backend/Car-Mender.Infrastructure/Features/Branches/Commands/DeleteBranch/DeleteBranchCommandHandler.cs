using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Car_Mender.Infrastructure.Features.Branches.Commands.DeleteBranch;

public class DeleteBranchCommandHandler(
	IBranchRepository branchRepository,
	ILogger<DeleteBranchCommandHandler> logger)
	: IRequestHandler<DeleteBranchCommand, Result>
{
	public async Task<Result> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty))
		{
			return Error.InvalidId;
		}

		var deleteBranchResult = await branchRepository.DeleteBranchAsync(request.Id, cancellationToken);
		if (deleteBranchResult.IsFailure)
		{
			logger.LogError("Could not remove branch with id: {Id}: {Message}", request.Id,
				deleteBranchResult.Error.Description);
		}

		return deleteBranchResult;
	}
}