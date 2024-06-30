using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Vehicles.DTOs;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Vehicles.Commands.UpdateVehicle;

public record UpdateVehicleCommand(Guid Id, JsonPatchDocument<UpdateVehicleDto> PatchDocument) : IRequest<Result>;