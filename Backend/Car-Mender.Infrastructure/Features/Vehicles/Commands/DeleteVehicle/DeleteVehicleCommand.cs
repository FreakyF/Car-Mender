using Car_Mender.Domain.Common;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Vehicles.Commands.DeleteVehicle;

public record DeleteVehicleCommand(Guid Id) : IRequest<Result>;