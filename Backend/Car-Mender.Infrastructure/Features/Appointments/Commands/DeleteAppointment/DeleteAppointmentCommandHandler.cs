using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Car_Mender.Infrastructure.Features.Appointments.Commands.DeleteAppointment;

public class DeleteAppointmentCommandHandler(
	IAppointmentRepository appointmentRepository,
	ILogger<DeleteAppointmentCommandHandler> logger)
	: IRequestHandler<DeleteAppointmentCommand, Result>
{
	public async Task<Result> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty)) return Error.InvalidId;

		var deleteAppointmentResult = await appointmentRepository.DeleteAppointmentAsync(request.Id, cancellationToken);
		if (deleteAppointmentResult.IsFailure)
			logger.LogError("Could not remove appointment with id: {Id}: {Message}", request.Id,
				deleteAppointmentResult.Error.Description);

		return deleteAppointmentResult;
	}
}