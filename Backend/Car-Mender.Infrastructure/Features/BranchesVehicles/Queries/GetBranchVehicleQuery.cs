using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.BranchesVehicles.DTOs;
using MediatR;

namespace Car_Mender.Infrastructure.Features.BranchesVehicles.Queries;

public record GetBranchVehicleQuery(Guid Id) : IRequest<Result<GetBranchVehicleDto>>;