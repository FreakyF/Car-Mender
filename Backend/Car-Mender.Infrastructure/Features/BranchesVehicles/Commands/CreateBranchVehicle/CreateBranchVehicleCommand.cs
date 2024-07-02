using Car_Mender.Domain.Common;
using MediatR;

namespace Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.CreateBranchVehicle;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateBranchVehicleCommand : IRequest<Result<Guid>>
{
	public Guid VehicleId { get; set; }
	public Guid BranchId { get; set; }
}