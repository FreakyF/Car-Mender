namespace Car_Mender.Domain.Entities;

public class Car : BaseEntity
{
    public Guid EngineId { get; init; }
    public required Engine Engine { get; init; }

    public required string Vin { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public required string Generation { get; set; }
    public required string Year { get; set; }
}