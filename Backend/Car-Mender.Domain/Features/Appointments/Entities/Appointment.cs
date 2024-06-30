using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Vehicles.Entities;

namespace Car_Mender.Domain.Features.Appointments.Entities;

public class Appointment : BaseEntity
{
	public Guid VehicleId { get; init; }
	public required Vehicle Vehicle { get; init; }
	public required DateTime Date { get; set; }
	public required string Description { get; set; }
	public required AppointmentStatus AppointmentStatus { get; set; }
}