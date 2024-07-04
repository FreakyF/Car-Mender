using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Vehicles.Entities;

namespace Car_Mender.Domain.Entities;

public class BranchVehicle : BaseEntity
{
	public required Guid VehicleId { get; init; }
	public Vehicle Vehicle { get; init; }

	public required Guid BranchId { get; init; }
	public Branch Branch { get; init; }
}