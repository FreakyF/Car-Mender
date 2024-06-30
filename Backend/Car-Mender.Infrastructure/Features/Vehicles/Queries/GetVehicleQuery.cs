using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Vehicles.DTOs;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Vehicles.Queries;

public record GetVehicleQuery(Guid Id) : IRequest<Result<GetVehicleDto>>;