using Car_Mender.Domain.Enums;

namespace Car_Mender.Domain.Features.Issues.DTOs;

public class UpdateIssueDto
{
	public Guid AppointmentId { get; init; }
	public Guid CreatorId { get; init; }
	public required ReporterType ReporterType { get; init; }
	public required string Description { get; init; }
	public required DateTime ReportedDate { get; init; }
	public required IssueStatus Status { get; init; }
}