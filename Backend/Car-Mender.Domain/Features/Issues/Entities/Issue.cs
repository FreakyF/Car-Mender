using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Appointments.Entities;
using Car_Mender.Domain.Features.Workers.Entities;

namespace Car_Mender.Domain.Features.Issues.Entities;

public class Issue : BaseEntity
{
	public required Guid AppointmentId { get; init; }
	public Appointment Appointment { get; init; }
	public required Guid CreatorId { get; init; }
	public Worker Creator { get; init; }
	public required ReporterType ReporterType { get; init; }
	public required string Description { get; set; }
	public required DateTime ReportedDate { get; set; }
	public required IssueStatus Status { get; set; }
}