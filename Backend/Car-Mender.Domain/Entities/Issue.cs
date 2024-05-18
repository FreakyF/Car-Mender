using Car_Mender.Domain.Enums;

namespace Car_Mender.Domain.Entities;

public class Issue : BaseEntity
{
    public Guid CarId { get; init; }
    public required Car Car { get; init; }

    public Guid CreatorId { get; init; }
    public required Worker Creator { get; init; }

    public required ReporterType ReporterType { get; set; }
    public required string Description { get; set; }
    public required DateTime ReportedDate { get; set; }
    public required IssueStatus Status { get; set; }
}