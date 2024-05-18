using Car_Mender.Domain.Enums;

namespace Car_Mender.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid CarId { get; init; }
    public required Vehicle Vehicle { get; init; }

    public required DateTime Date { get; set; }
    public required string Description { get; set; }
    public required AppointmentStatus AppointmentStatus { get; set; }
}