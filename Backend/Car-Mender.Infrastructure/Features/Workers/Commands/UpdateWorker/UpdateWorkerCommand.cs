using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Workers.DTOs;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Workers.Commands.UpdateWorker;

public record UpdateWorkerCommand(Guid Id, JsonPatchDocument<UpdateWorkerDto> PatchDocument) : IRequest<Result>;