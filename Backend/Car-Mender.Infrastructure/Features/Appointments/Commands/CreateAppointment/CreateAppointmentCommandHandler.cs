using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.Entities;
using Car_Mender.Domain.Features.Appointments.Repository;
using Car_Mender.Domain.Features.Vehicles.Errors;
using Car_Mender.Domain.Features.Vehicles.Repository;
using FluentValidation;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler(
	IAppointmentRepository appointmentRepository,
	IVehicleRepository vehicleRepository,
	IValidator<CreateAppointmentCommand> validator,
	IMapper mapper
) : IRequestHandler<CreateAppointmentCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
	{
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

		var appointment = mapper.Map<Appointment>(request);
		return await appointmentRepository.CreateAppointmentAsync(appointment, cancellationToken);
	}
}