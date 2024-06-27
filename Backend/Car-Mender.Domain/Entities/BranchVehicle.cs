using Car_Mender.Domain.Features.Branches.Entities;

namespace Car_Mender.Domain.Entities;

public class BranchVehicle : BaseEntity
{
	public Guid VehicleId { get; init; }
	public required Vehicle Vehicle { get; init; }

	public Guid BranchId { get; init; }
	public required Branch Branch { get; init; }
}