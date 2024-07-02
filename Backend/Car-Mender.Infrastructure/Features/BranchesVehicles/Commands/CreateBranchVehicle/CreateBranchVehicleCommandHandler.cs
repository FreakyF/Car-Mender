using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Branches.Errors;
using Car_Mender.Domain.Features.Branches.Repository;
using Car_Mender.Domain.Features.Vehicles.Errors;
using Car_Mender.Domain.Features.Vehicles.Repository;
using Car_Mender.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.CreateBranchVehicle;

public class CreateBranchVehicleCommandHandler(
	IBranchVehicleRepository branchVehicleRepository,
	IBranchRepository branchRepository,
	IVehicleRepository vehicleRepository,
	IValidator<CreateBranchVehicleCommand> validator,
	IMapper mapper
) : IRequestHandler<CreateBranchVehicleCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateBranchVehicleCommand request, CancellationToken cancellationToken)
	{
		var branchExistsResult = await branchRepository.ExistsAsync(request.BranchId);
		if (branchExistsResult.IsFailure) return Result<Guid>.Failure(branchExistsResult.Error);

		var branchExists = branchExistsResult.Value;
		if (!branchExists) return BranchErrors.CouldNotBeFound;

		var vehicleExistsResult = await vehicleRepository.ExistsAsync(request.VehicleId);
		if (vehicleExistsResult.IsFailure) return Result<Guid>.Failure(vehicleExistsResult.Error);

		var vehicleExists = vehicleExistsResult.Value;
		if (!vehicleExists) return VehicleErrors.CouldNotBeFound;

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var branchVehicleEntity = mapper.Map<BranchVehicle>(request);
		return await branchVehicleRepository.CreateBranchVehicleAsync(branchVehicleEntity, cancellationToken);
	}
}