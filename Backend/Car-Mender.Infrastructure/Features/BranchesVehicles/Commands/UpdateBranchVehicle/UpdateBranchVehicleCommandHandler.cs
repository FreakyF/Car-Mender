using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Branches.Errors;
using Car_Mender.Domain.Features.Branches.Repository;
using Car_Mender.Domain.Features.BranchesVehicles.DTOs;
using Car_Mender.Domain.Features.Vehicles.Errors;
using Car_Mender.Domain.Features.Vehicles.Repository;
using Car_Mender.Domain.Features.Workers.Errors;
using Car_Mender.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.UpdateBranchVehicle;

public class UpdateBranchVehicleCommandHandler(
	IBranchVehicleRepository branchVehicleRepository,
	IBranchRepository branchRepository,
	IVehicleRepository vehicleRepository,
	IMapper mapper
) : IRequestHandler<UpdateBranchVehicleCommand, Result>
{
	public async Task<Result> Handle(UpdateBranchVehicleCommand request, CancellationToken cancellationToken)
	{
		var branchIdUpdateOperation = GetUpdateBranchIdOperation(request);
		var branchIdBeingUpdated = branchIdUpdateOperation is not null;
		if (branchIdBeingUpdated)
		{
			var branchId = Guid.Parse(branchIdUpdateOperation!.value.ToString()!);
			if (branchId.Equals(Guid.Empty)) return Error.InvalidId;
			var branchExistsResult = await branchRepository.ExistsAsync(branchId);
			if (!branchExistsResult.Value) return BranchErrors.CouldNotBeFound;
		}

		var vehicleIdUpdateOperation = GetUpdateVehicleIdOperation(request);
		var vehicleIdBeingUpdated = vehicleIdUpdateOperation is not null;
		if (vehicleIdBeingUpdated)
		{
			var vehicleId = Guid.Parse(vehicleIdUpdateOperation!.value.ToString()!);
			if (vehicleId.Equals(Guid.Empty)) return Error.InvalidId;
			var vehicleExistsResult = await vehicleRepository.ExistsAsync(vehicleId);
			if (!vehicleExistsResult.Value) return VehicleErrors.CouldNotBeFound;
		}

		var branchVehicleExistResult = await branchVehicleRepository.ExistsAsync(request.Id);
		if (!branchVehicleExistResult.Value) return WorkerErrors.CouldNotBeFound;

		var getBranchVehicleResult =
			await branchVehicleRepository.GetBranchVehicleByIdAsync(request.Id, cancellationToken);
		var patchDoc = mapper.Map<JsonPatchDocument<BranchVehicle>>(request.PatchDocument);
		patchDoc.ApplyTo(getBranchVehicleResult.Value!);
		await branchVehicleRepository.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}

	private static Operation<UpdateBranchVehicleDto>? GetUpdateBranchIdOperation(UpdateBranchVehicleCommand request)
	{
		return request.PatchDocument.Operations
			.Find(op =>
				string.Equals(op.path, $"/{nameof(UpdateBranchVehicleDto.BranchId)}",
					StringComparison.InvariantCultureIgnoreCase)
			);
	}

	private static Operation<UpdateBranchVehicleDto>? GetUpdateVehicleIdOperation(UpdateBranchVehicleCommand request)
	{
		return request.PatchDocument.Operations
			.Find(op =>
				string.Equals(op.path, $"/{nameof(UpdateBranchVehicleDto.VehicleId)}",
					StringComparison.InvariantCultureIgnoreCase)
			);
	}
}