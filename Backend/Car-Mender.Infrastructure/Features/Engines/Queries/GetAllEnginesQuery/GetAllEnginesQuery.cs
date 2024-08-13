using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.DTOs;
using Car_Mender.Domain.Features.Engines.DTOs;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Engines.Queries.GetAllEnginesQuery;

public record GetAllEnginesQuery(Guid VehicleId) : IRequest<Result<List<GetEngineDto>>>;