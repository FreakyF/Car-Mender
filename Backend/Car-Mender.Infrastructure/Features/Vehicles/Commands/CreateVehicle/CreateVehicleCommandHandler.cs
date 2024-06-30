using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.Errors;
using Car_Mender.Domain.Features.Engines.Repository;
using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Domain.Features.Vehicles.Repository;
using FluentValidation;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler(
	IVehicleRepository vehicleRepository,
	IEngineRepository engineRepository,
	IValidator<CreateVehicleCommand> validator,
	IMapper mapper
) : IRequestHandler<CreateVehicleCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
	{
		var engineExistResult = await engineRepository.ExistsAsync(request.EngineId);
		if (engineExistResult.IsFailure) return Result<Guid>.Failure(engineExistResult.Error);

		var engineExists = engineExistResult.Value;
		if (!engineExists) return EngineErrors.CouldNotBeFound;

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var vehicleEntity = mapper.Map<Vehicle>(request);
		return await vehicleRepository.CreateVehicleAsync(vehicleEntity, cancellationToken);
	}
}