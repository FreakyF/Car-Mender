using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Issues.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Car_Mender.Infrastructure.Features.Issues.Commands.DeleteIssue;

public class DeleteIssueCommandHandler(
	IIssueRepository workerRepository,
	ILogger<DeleteIssueCommandHandler> logger)
	: IRequestHandler<DeleteIssueCommand, Result>
{
	public async Task<Result> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty)) return Error.InvalidId;

		var deleteIssueResult = await workerRepository.DeleteIssueAsync(request.Id, cancellationToken);
		if (deleteIssueResult.IsFailure)
			logger.LogError("Could not remove issue with id: {Id}: {Message}", request.Id,
				deleteIssueResult.Error.Description);

		return deleteIssueResult;
	}
}