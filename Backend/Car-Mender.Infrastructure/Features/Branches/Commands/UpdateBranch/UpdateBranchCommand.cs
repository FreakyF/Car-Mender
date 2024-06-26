using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.DTOs;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Branches.Commands.UpdateBranch;

public record UpdateBranchCommand(Guid Id, JsonPatchDocument<UpdateBranchDto> PatchDocument) : IRequest<Result>;