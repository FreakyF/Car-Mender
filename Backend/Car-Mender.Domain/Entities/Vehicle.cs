namespace Car_Mender.Domain.Entities;

public class Vehicle : BaseEntity
{
    public Guid EngineId { get; init; }
    public required Engine Engine { get; init; }

    public required string Vin { get; init; }
    public required string Make { get; init; }
    public required string Model { get; init; }
    public required string Generation { get; init; }
    public required uint Year { get; init; }
}