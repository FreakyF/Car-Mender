using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.DTOs;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Engines.Commands.UpdateEngine;

public record UpdateEngineCommand(Guid Id, JsonPatchDocument<UpdateEngineDto> PatchDocument) : IRequest<Result>;