using Car_Mender.Domain.Common;
using MediatR;

namespace Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.DeleteBranchVehicle;

public record DeleteBranchVehicleCommand(Guid Id) : IRequest<Result>;