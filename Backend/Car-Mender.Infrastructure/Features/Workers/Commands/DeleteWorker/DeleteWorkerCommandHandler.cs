using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Workers.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Car_Mender.Infrastructure.Features.Workers.Commands.DeleteWorker;

public class DeleteWorkerCommandHandler(
	IWorkerRepository workerRepository,
	ILogger<DeleteWorkerCommandHandler> logger)
	: IRequestHandler<DeleteWorkerCommand, Result>
{
	public async Task<Result> Handle(DeleteWorkerCommand request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty))
		{
			return Error.InvalidId;
		}

		var deleteWorkerResult = await workerRepository.DeleteWorkerAsync(request.Id, cancellationToken);
		if (deleteWorkerResult.IsFailure)
		{
			logger.LogError("Could not remove worker with id: {Id}: {Message}", request.Id,
				deleteWorkerResult.Error.Description);
		}

		return deleteWorkerResult;
	}
}