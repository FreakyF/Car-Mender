using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Issues.DTOs;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Issues.Commands.UpdateIssue;

public record UpdateIssueCommand(Guid Id, JsonPatchDocument<UpdateIssueDto> PatchDocument) : IRequest<Result>;