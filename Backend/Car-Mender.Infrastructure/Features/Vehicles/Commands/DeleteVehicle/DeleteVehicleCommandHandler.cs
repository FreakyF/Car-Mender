using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Vehicles.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Car_Mender.Infrastructure.Features.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler(
	IVehicleRepository vehicleRepository,
	ILogger<DeleteVehicleCommandHandler> logger)
	: IRequestHandler<DeleteVehicleCommand, Result>
{
	public async Task<Result> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty)) return Error.InvalidId;

		var deleteVehicleResult = await vehicleRepository.DeleteVehicleAsync(request.Id, cancellationToken);
		if (deleteVehicleResult.IsFailure)
			logger.LogError("Could not remove vehicle with id: {Id}: {Message}", request.Id,
				deleteVehicleResult.Error.Description);

		return deleteVehicleResult;
	}
}