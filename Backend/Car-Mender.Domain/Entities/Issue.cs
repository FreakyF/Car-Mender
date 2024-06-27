using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Workers.Entities;

namespace Car_Mender.Domain.Entities;

public class Issue : BaseEntity
{
	public Guid AppointmentId { get; init; }
	public required Appointment Appointment { get; init; }

	public Guid CreatorId { get; init; }
	public required Worker Creator { get; init; }

	public required ReporterType ReporterType { get; init; }
	public required string Description { get; set; }
	public required DateTime ReportedDate { get; set; }
	public required IssueStatus Status { get; set; }
}