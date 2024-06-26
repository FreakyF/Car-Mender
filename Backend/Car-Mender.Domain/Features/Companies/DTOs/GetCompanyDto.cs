using Car_Mender.Domain.Models;

namespace Car_Mender.Domain.Features.Companies.DTOs;

public class GetCompanyDto
{
	public required Guid Id { get; init; }
	public required string Email { get; init; }
	public required string Name { get; init; }
	public required Address Address { get; init; }
	public required string Phone { get; init; }
	public required string Nip { get; init; }
}