using Car_Mender.Domain.Enums;

namespace Car_Mender.Domain.Features.Appointments.DTOs;

public class GetAppointmentDto
{
	public Guid VehicleId { get; init; }
	public required DateTime Date { get; init; }
	public required string Description { get; init; }
	public required AppointmentStatus AppointmentStatus { get; init; }
}