using Car_Mender.Domain.Common;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Workers.Commands.DeleteWorker;

public record DeleteWorkerCommand(Guid Id) : IRequest<Result>;