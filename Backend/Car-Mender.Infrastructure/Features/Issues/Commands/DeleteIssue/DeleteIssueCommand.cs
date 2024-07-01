using Car_Mender.Domain.Common;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Issues.Commands.DeleteIssue;

public record DeleteIssueCommand(Guid Id) : IRequest<Result>;