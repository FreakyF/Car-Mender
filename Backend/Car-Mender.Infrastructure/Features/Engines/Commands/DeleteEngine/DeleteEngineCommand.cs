using Car_Mender.Domain.Common;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Engines.Commands.DeleteEngine;

public record DeleteEngineCommand(Guid Id) : IRequest<Result>;