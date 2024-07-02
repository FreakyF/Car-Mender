namespace Car_Mender.Domain.Features.BranchesVehicles.DTOs;

public class UpdateBranchVehicleDto
{
	public Guid VehicleId { get; init; }
	public Guid BranchId { get; init; }
}