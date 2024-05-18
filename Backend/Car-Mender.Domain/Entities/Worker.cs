namespace Car_Mender.Domain.Entities;

public class Worker : BaseEntity
{
    public Guid BranchId { get; init; }
    public required Branch Branch { get; init; }

    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Phone { get; set; }
}