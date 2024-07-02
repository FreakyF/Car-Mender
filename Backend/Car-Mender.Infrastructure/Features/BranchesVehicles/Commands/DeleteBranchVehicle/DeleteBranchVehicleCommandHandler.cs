using Car_Mender.Domain.Common;
using Car_Mender.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.DeleteBranchVehicle;

public class DeleteBranchVehicleCommandHandler(
	IBranchVehicleRepository branchVehicleRepository,
	ILogger<DeleteBranchVehicleCommandHandler> logger)
	: IRequestHandler<DeleteBranchVehicleCommand, Result>
{
	public async Task<Result> Handle(DeleteBranchVehicleCommand request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty)) return Error.InvalidId;

		var deleteBranchVehicleResult = await branchVehicleRepository.DeleteBranchVehicleAsync(request.Id, cancellationToken);
		if (deleteBranchVehicleResult.IsFailure)
			logger.LogError("Could not remove branch vehicle with id: {Id}: {Message}", request.Id,
				deleteBranchVehicleResult.Error.Description);

		return deleteBranchVehicleResult;
	}
}