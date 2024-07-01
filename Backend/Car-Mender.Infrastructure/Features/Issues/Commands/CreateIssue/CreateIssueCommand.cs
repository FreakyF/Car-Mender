using Car_Mender.Domain.Common;
using Car_Mender.Domain.Enums;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Issues.Commands.CreateIssue;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateIssueCommand : IRequest<Result<Guid>>
{
	public Guid AppointmentId { get; set; }
	public Guid CreatorId { get; set; }
	public required ReporterType ReporterType { get; set; }
	public required string Description { get; set; }
	public required DateTime ReportedDate { get; set; }
	public required IssueStatus Status { get; set; }
}