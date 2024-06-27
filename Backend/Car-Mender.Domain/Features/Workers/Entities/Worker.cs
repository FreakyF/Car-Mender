using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Branches.Entities;

namespace Car_Mender.Domain.Features.Workers.Entities;

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