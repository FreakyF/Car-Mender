using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Workers.DTOs;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Workers.Queries;

public record GetAllWorkersQuery(Guid BranchId) : IRequest<Result<List<GetWorkerDto>>>;