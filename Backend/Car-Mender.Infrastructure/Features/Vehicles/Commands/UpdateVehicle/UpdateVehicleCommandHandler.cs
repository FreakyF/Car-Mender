using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.Errors;
using Car_Mender.Domain.Features.Engines.Repository;
using Car_Mender.Domain.Features.Vehicles.DTOs;
using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Domain.Features.Vehicles.Errors;
using Car_Mender.Domain.Features.Vehicles.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Car_Mender.Infrastructure.Features.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandHandler(
	IVehicleRepository vehicleRepository,
	IEngineRepository engineRepository,
	IValidator<UpdateVehicleCommand> validator,
	IMapper mapper
) : IRequestHandler<UpdateVehicleCommand, Result>
{
	public async Task<Result> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
	{
		var engineIdUpdateOperation = GetUpdateEngineIdOperation(request);
		var branchIdBeingUpdated = engineIdUpdateOperation is not null;
		if (branchIdBeingUpdated)
		{
			var engineId = Guid.Parse(engineIdUpdateOperation!.value.ToString()!);
			if (engineId.Equals(Guid.Empty)) return Error.InvalidId;
			var engineExistsResult = await engineRepository.ExistsAsync(engineId);
			if (!engineExistsResult.Value) return EngineErrors.CouldNotBeFound;
		}

		var vehicleExistsResult = await vehicleRepository.ExistsAsync(request.Id);
		if (!vehicleExistsResult.Value) return VehicleErrors.CouldNotBeFound;

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var getVehicleResult = await vehicleRepository.GetVehicleByIdAsync(request.Id, cancellationToken);
		var patchDoc = mapper.Map<JsonPatchDocument<Vehicle>>(request.PatchDocument);
		patchDoc.ApplyTo(getVehicleResult.Value!);
		await vehicleRepository.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}

	private static Operation<UpdateVehicleDto>? GetUpdateEngineIdOperation(UpdateVehicleCommand request)
	{
		return request.PatchDocument.Operations
			.FirstOrDefault(op =>
				string.Equals(op.path, $"/{nameof(UpdateVehicleDto.EngineId)}",
					StringComparison.InvariantCultureIgnoreCase)
			);
	}
}