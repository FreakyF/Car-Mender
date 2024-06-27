using Car_Mender.Domain.Enums;

namespace Car_Mender.Domain.Features.Engines.DTOs;

public class UpdateEngineDto
{
	public required string EngineCode { get; init; }
	public required uint Displacement { get; init; }
	public required uint PowerKw { get; init; }
	public required uint TorqueNm { get; init; }
	public required FuelType FuelType { get; init; }
}