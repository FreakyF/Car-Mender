using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.DTOs;
using Car_Mender.Domain.Features.Appointments.Entities;
using Car_Mender.Domain.Features.Appointments.Errors;
using Car_Mender.Domain.Features.Appointments.Repository;
using Car_Mender.Domain.Features.Vehicles.Errors;
using Car_Mender.Domain.Features.Vehicles.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Car_Mender.Infrastructure.Features.Appointments.Commands.UpdateAppointment;

public class UpdateAppointmentCommandHandler(
	IAppointmentRepository appointmentRepository,
	IVehicleRepository vehicleRepository,
	IValidator<UpdateAppointmentCommand> validator,
	IMapper mapper
) : IRequestHandler<UpdateAppointmentCommand, Result>
{
	public async Task<Result> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
	{
		var vehicleIdUpdateOperation = GetUpdateVehicleIdOperation(request);
		var vehicleIdBeingUpdated = vehicleIdUpdateOperation is not null;
		if (vehicleIdBeingUpdated)
		{
			var vehicleId = Guid.Parse(vehicleIdUpdateOperation!.value.ToString()!);
			if (vehicleId.Equals(Guid.Empty)) return Error.InvalidId;
			var vehicleExistsResult = await vehicleRepository.ExistsAsync(vehicleId);
			if (!vehicleExistsResult.Value) return VehicleErrors.CouldNotBeFound;
		}

		var appointmentExistsResult = await appointmentRepository.ExistsAsync(request.Id);
		if (!appointmentExistsResult.Value) return AppointmentErrors.CouldNotBeFound;

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var getAppointmentResult = await appointmentRepository.GetAppointmentByIdAsync(request.Id, cancellationToken);
		var patchDoc = mapper.Map<JsonPatchDocument<Appointment>>(request.PatchDocument);
		patchDoc.ApplyTo(getAppointmentResult.Value!);
		await appointmentRepository.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}

	private static Operation<UpdateAppointmentDto>? GetUpdateVehicleIdOperation(UpdateAppointmentCommand request)
	{
		return request.PatchDocument.Operations
			.Find(op =>
				string.Equals(op.path, $"/{nameof(UpdateAppointmentDto.VehicleId)}",
					StringComparison.InvariantCultureIgnoreCase)
			);
	}
}