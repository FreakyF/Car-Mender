using FluentValidation;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
	public UpdateVehicleCommandValidator()
	{
		// TODO :)
	}
}