using Car_Mender.Domain.Models;

namespace Car_Mender.Domain.Features.Companies.DTOs;

public class CreateCompanyDto
{
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required Address Address { get; init; }
    public required string Phone { get; init; }
    public required string Nip { get; init; }
}