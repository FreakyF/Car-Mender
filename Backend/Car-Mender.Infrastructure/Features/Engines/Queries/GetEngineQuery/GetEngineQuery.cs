using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.DTOs;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Engines.Queries.GetEngineQuery;

public record GetEngineQuery(Guid Id) : IRequest<Result<GetEngineDto>>;