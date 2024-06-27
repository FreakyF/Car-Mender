using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Enums;

namespace Car_Mender.Domain.Features.Engines.Entities;

public class Engine : BaseEntity
{
    public required string EngineCode { get; init; }
    public required uint Displacement { get; set; }
    public required uint PowerKw { get; set; }
    public required uint TorqueNm { get; set; }
    public required FuelType FuelType { get; set; }
}