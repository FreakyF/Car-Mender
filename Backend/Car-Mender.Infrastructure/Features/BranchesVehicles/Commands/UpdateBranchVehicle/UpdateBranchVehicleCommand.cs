using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.BranchesVehicles.DTOs;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.UpdateBranchVehicle;

public record UpdateBranchVehicleCommand(Guid Id, JsonPatchDocument<UpdateBranchVehicleDto> PatchDocument) : IRequest<Result>;