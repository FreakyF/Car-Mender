using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Issues.DTOs;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Issues.Queries;

public record GetAllIssuesQuery(Guid VehicleId) : IRequest<Result<List<GetIssueDto>>>;