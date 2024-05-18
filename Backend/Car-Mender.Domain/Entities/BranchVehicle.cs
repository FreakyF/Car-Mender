namespace Car_Mender.Domain.Entities;

public class BranchVehicle
{
    public Guid CarId { get; init; }
    public required Car Car { get; init; }

    public Guid BranchId { get; init; }
    public required Branch Branch { get; init; }
}