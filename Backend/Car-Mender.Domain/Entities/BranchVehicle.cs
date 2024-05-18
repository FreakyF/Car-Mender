namespace Car_Mender.Domain.Entities;

public class BranchVehicle
{
    public Guid CarId { get; init; }
    public required Vehicle Vehicle { get; init; }

    public Guid BranchId { get; init; }
    public required Branch Branch { get; init; }
}