using Car_Mender.Domain.Features.Engines.Entities;

namespace Car_Mender.Domain.Features.Vehicles.DTOs;

public class GetVehicleDto
{
	public Guid EngineId { get; init; }
	public required string Vin { get; init; }
	public required string Make { get; init; }
	public required string Model { get; init; }
	public required string Generation { get; init; }
	public required uint Year { get; init; }
}