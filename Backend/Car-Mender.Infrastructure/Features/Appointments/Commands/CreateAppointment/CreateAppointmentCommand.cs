using Car_Mender.Domain.Common;
using Car_Mender.Domain.Enums;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Appointments.Commands.CreateAppointment;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateAppointmentCommand : IRequest<Result<Guid>>
{
	public Guid VehicleId { get; set; }
	public required DateTime Date { get; set; }
	public required string Description { get; set; }
	public required AppointmentStatus AppointmentStatus { get; set; }
}