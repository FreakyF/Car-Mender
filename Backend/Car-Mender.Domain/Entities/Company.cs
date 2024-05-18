using Car_Mender.Domain.Models;

namespace Car_Mender.Domain.Entities;

public class Company : BaseEntity
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required Address Address { get; set; }
    public required string Phone { get; set; }
    public required string Nip { get; set; }
}